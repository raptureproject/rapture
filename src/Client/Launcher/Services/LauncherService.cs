// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.Win32.SafeHandles;
using Rapture.Client.Core.Utilities;
using System.Diagnostics;
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
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var gamePath = GameUtilities.GetGameInstallPath();

        if (gamePath is null)
        {
            PInvoke.MessageBox(HWND.Null, "A Final Fantasy XIV 1.0 Install Was Not Found!", "Final Fantasy XIV Not Installed", MESSAGEBOX_STYLE.MB_ICONERROR);
            Environment.Exit(1);
        }

        StartBoot(gamePath);

        return base.StartAsync(cancellationToken);
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            if (GetMonitoredProcesses().Count == 0)
            {
                await Task.Delay(500, stoppingToken);

                // Double check no processes are running.
                if (GetMonitoredProcesses().Count == 0)
                {
                    Environment.Exit(0);
                }
            }
        }
    }

    /// <inheritdoc/>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        var gameProcesses = GetMonitoredProcesses();

        foreach (var process in gameProcesses)
        {
            try
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }
            catch { }
        }
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

        var bootHash = Convert.ToHexString(HashingUtilities.SHA1HashFile(bootPath));

        // Version 2010.07.10.0000
        if (bootHash == "999EFB09D7D94C9C8106A75688CF3BED7C1FBA84")
        {
            PatchBoot(process.dwProcessId);
        }

        _ = PInvoke.ResumeThread(process.hThread);
    }

    private static void PatchBoot(uint processId)
    {
        var processHandle = PInvoke.OpenProcess_SafeHandle(PROCESS_ACCESS_RIGHTS.PROCESS_ALL_ACCESS, false, processId);
        uint baseOffset = 0x400000;

        // SetProcessAffinityMask
        ApplyPatch(processHandle, baseOffset + 0x49CC, [.. Enumerable.Repeat<byte>(0x90, 30)]);

        // SQEX::CDev::Engine::Vfx::Qix::Thread::ThreadManager::CreateEffectThread
        ApplyPatch(processHandle, baseOffset + 0x6937BF, [0xB5, 0x01]);

        // rsa_verify
        ApplyPatch(processHandle, baseOffset + 0x5DF64, [0x01, 0x00, 0x00, 0x00]);

        // ver01.ffxiv.com -> 127.0.0.1
        ApplyPatch(processHandle, baseOffset + 0x8E62DC, Encoding.ASCII.GetBytes("127.0.0.1\0\0\0\0\0\0"));
    }

    private static unsafe void ApplyPatch(SafeFileHandle processHandle, nuint address, byte[] patchData)
    {
        var patchSize = (uint)patchData.Length;
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

    private static List<Process> GetMonitoredProcesses()
    {
        return
        [
            .. Process.GetProcessesByName("ffxivboot"),
            .. Process.GetProcessesByName("ffxivupdater"),
            .. Process.GetProcessesByName("ffxivlogin"),
            .. Process.GetProcessesByName("ffxivgame")
        ];
    }
}
