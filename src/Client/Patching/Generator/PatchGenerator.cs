// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Patching.Generator.ZiPatch;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Rapture.Client.Patching.Generator;

/// <summary>
/// Provides methods to generate and apply binary patches to Final Fantasy XIV executable files for specific patch versions.
/// </summary>
public static class PatchGenerator
{
    /// <summary>
    /// Generates all required patches by invoking both the boot and game patch generation processes.
    /// </summary>
    public static void GenerateAllPatches()
    {
        GenerateBootPatch();
        GenerateGamePatch();

        PInvoke.MessageBox(HWND.Null, "Patches generated successfully!", "Rapture Patch Generator", MESSAGEBOX_STYLE.MB_ICONERROR);
    }

    /// <summary>
    /// Generates and saves a boot patch with predefined modifications for the FFXIV boot executable.
    /// </summary>
    public static void GenerateBootPatch()
    {
        using var bootPatch = new Patch(2010, 9, 18, 1, PatchType.Boot);

        var bootFile = bootPatch.AddFile("ffxivboot.exe");

        // SetProcessAffinityMask
        bootFile.ApplyPatch(0x3D7B, [.. Enumerable.Repeat<byte>(0x90, 30)]);

        // SQEX::CDev::Engine::Vfx::Qix::Thread::ThreadManager::CreateEffectThread
        bootFile.ApplyPatch(0x70056B, [0xB5, 0x01]);
        bootFile.ApplyPatch(0x700613, [0xB5, 0x01]);

        // rsa_verify
        bootFile.ApplyPatch(0x64324, [0x01, 0x00, 0x00, 0x00]);

        // ver01.ffxiv.com -> 127.0.0.1
        bootFile.ApplyPatch(0x966404, Encoding.ASCII.GetBytes("127.0.0.1\0\0\0\0\0\0\0"));

        bootPatch.Save();
    }

    /// <summary>
    /// Generates and applies a set of binary patches to game executable files for a specific game patch version.
    /// </summary>
    public static void GenerateGamePatch()
    {
        using var gamePatch = new Patch(2012, 9, 19, 2, PatchType.Game);

        var loginFile = gamePatch.AddFile("ffxivlogin.exe");

        // http://account.square-enix.com/account/content/ffxivlogin -> http://127.0.0.1:54998/account/content/ffxivlogin
        loginFile.ApplyPatch(0x53EA0, [
            0xF3, 0x57, 0xFC, 0xFE, 0xD0, 0x59, 0x13, 0xF4,
            0x29, 0xF5, 0x91, 0x01, 0x5B, 0x32, 0x7D, 0x4A,
            0x20, 0x68, 0xD3, 0xFE, 0x02, 0x58, 0xB3, 0xAD,
            0x22, 0xE6, 0x6F, 0x10, 0xEA, 0x76, 0x34, 0x0B,
            0x7E, 0x2D, 0xD2, 0xAC, 0xD7, 0xC3, 0xD3, 0xC1,
            0x4D, 0x96, 0xED, 0xD4, 0xCC, 0x5E, 0x0D, 0xF5,
            0x7E, 0x35, 0x99, 0xB9, 0x57
        ]);

        var gameFile = gamePatch.AddFile("ffxivgame.exe");

        // SetProcessAffinityMask
        gameFile.ApplyPatch(0x3698, [.. Enumerable.Repeat<byte>(0x90, 30)]);

        // SQEX::CDev::Engine::Vfx::Qix::Thread::ThreadManager::CreateEffectThread
        gameFile.ApplyPatch(0x7B952B, [0xB5, 0x01]);
        gameFile.ApplyPatch(0x7B95D3, [0xB5, 0x01]);

        // lobby01.ffxiv.com -> 127.0.0.1
        gameFile.ApplyPatch(0x966404, Encoding.ASCII.GetBytes("127.0.0.1\0\0\0\0\0\0\0\0\0\0\0"));

        gamePatch.Save();
    }
}
