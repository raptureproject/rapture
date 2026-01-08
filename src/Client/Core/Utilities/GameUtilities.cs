// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.Win32;

namespace Rapture.Client.Core.Utilities;

/// <summary>
/// Provides utility methods for retrieving game-related information from the system.
/// </summary>
public static class GameUtilities
{
    /// <summary>
    /// Retrieves the full installation path of the game from the Windows registry.
    /// </summary>
    /// <returns>A string containing the full path to the game's installation directory if found; otherwise, null.</returns>
    public static string? GetGameInstallPath()
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
}
