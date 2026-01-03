// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;
using Windows.Win32.System.Threading;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Rapture.Client.Launcher.Services;

/// <summary>
/// Provides a background service that locates and launches the Final Fantasy XIV 1.0 game client when the application starts.
/// </summary>
public class LauncherService : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Delay to ensure server starts.
        await Task.Delay(100, stoppingToken);

        LaunchGame();
    }

    private static void LaunchGame()
    {
        var gamePath = GetGameInstallPath();

        if (gamePath is null)
        {
            PInvoke.MessageBox(HWND.Null, "A Final Fantasy XIV 1.0 Install Was Not Found!", "Final Fantasy XIV Not Installed", MESSAGEBOX_STYLE.MB_ICONERROR);
            Environment.Exit(1);
        }

        StartBoot(gamePath);
    }

    private static string? GetGameInstallPath()
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{F2C4E6E0-EB78-4824-A212-6DF6AF0E8E82}");

        if (key is null)
        {
            return null;
        }

        if (key.GetValue("InstallLocation") is not string installLocation ||
            key.GetValue("DisplayName") is not string displayName)
        {
            return null;
        }

        return Path.Combine(installLocation, displayName);
    }

    private static unsafe void StartBoot(string bootDirectory)
    {
        var bootPath = Path.Combine(bootDirectory, "ffxivboot.exe");

        var success = PInvoke.CreateProcess(bootPath, null, null, false, PROCESS_CREATION_FLAGS.CREATE_SUSPENDED, null, bootDirectory, new STARTUPINFOW(), out var process);

        if (!success)
        {
            PInvoke.MessageBox(HWND.Null, "Failed To Launch Final Fantasy XIV!", "Final Fantasy XIV Launch Failed", MESSAGEBOX_STYLE.MB_ICONERROR);
            Environment.Exit(1);
        }

        var bootHash = HashFile(bootPath);

        // Version 2010.07.10.0000
        if (bootHash == "999EFB09D7D94C9C8106A75688CF3BED7C1FBA84")
        {
            PatchBoot(process.dwProcessId);
        }

        _ = PInvoke.ResumeThread(process.hThread);
    }

    [SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "This is for file hashing.")]
    private static string HashFile(string path)
    {
        using var file = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(file);

        return Convert.ToHexString(hash);
    }

    private static void PatchBoot(uint processId)
    {
        var processHandle = PInvoke.OpenProcess_SafeHandle(PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS, false, processId);
        uint baseOffset = 0x400000;

        // ver01.ffxiv.com -> 127.0.0.1
        ApplyPatch(processHandle, baseOffset + 0x8E62DC, Encoding.ASCII.GetBytes("127.0.0.1\0\0\0\0\0\0\0"), 16);

        // rsa_verify
        ApplyPatch(processHandle, baseOffset + 0x5DF64, [0x01, 0x00, 0x00, 0x00], 4);
    }

    private static unsafe void ApplyPatch(SafeFileHandle processHandle, nuint address, byte[] patchData, nuint patchSize)
    {
        var unprotectResult = PInvoke.VirtualProtectEx(processHandle, (void*)address, patchSize, PAGE_PROTECTION_FLAGS.PAGE_EXECUTE_READWRITE, out var oldProtect);
        var writeResult = PInvoke.WriteProcessMemory(processHandle, (void*)address, patchData);
        var reprotectResult = PInvoke.VirtualProtectEx(processHandle, (void*)address, patchSize, oldProtect, out _);

        if (!(unprotectResult && writeResult && reprotectResult))
        {
            PInvoke.TerminateProcess(processHandle, 1);
            PInvoke.MessageBox(HWND.Null, "Failed To Start ffxivboot.exe!", "Final Fantasy XIV Launch Failed", MESSAGEBOX_STYLE.MB_ICONERROR);
            Environment.Exit(1);
        }
    }
}
