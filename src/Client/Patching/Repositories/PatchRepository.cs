// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent.Client;
using Rapture.Client.Patching.Models;
using System.Collections.ObjectModel;

namespace Rapture.Client.Patching.Repositories;

/// <summary>
/// Provides access to the collection of available patch metadata entries for the game.
/// </summary>
public class PatchRepository(ClientEngine clientEngine)
{
    /// <summary>
    /// Gets the collection of patch metadata entries available for the game.
    /// </summary>
    public ReadOnlyCollection<PatchInfo> PatchInfo => GetPatchInfo();

    private ReadOnlyCollection<PatchInfo> GetPatchInfo()
    {
        var patchInfo = clientEngine.Torrents.Select(torrent =>
        {
            var patchFile = torrent.Files.First();

            var type = patchFile.FullPath.StartsWith("2d2a390f") ? "boot" : "game";
            var version = Path.GetFileNameWithoutExtension(patchFile.Path)[1..];
            var versionParts = version.Split('.');
            var buildTime = new DateOnly(int.Parse(versionParts[0]), int.Parse(versionParts[1]), int.Parse(versionParts[2]));
            var repositoryHash = patchFile.Path.Split("\\")[0];
            var fileSize = (ulong)patchFile.Length;

            return new PatchInfo()
            {
                Platform = "win32",
                Channel = "release",
                Type = type,
                Version = version,
                BuildTime = buildTime,
                RepositoryHash = repositoryHash,
                FileSize = fileSize
            };
        });

        return new([.. patchInfo]);
    }
}
