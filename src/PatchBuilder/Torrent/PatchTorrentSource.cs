// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent;

namespace Rapture.PatchBuilder.Torrent;

/// <summary>
/// Initializes a new instance of the PatchTorrentSource class using the specified full patch file path and relative patch path.
/// </summary>
/// <param name="fullPatchPath">The full file system path to the patch file. This path is used to locate the patch file on disk. Cannot be null or empty.</param>
/// <param name="patchPath">The relative path to the patch file as it should appear in the torrent.</param>
public class PatchTorrentSource(string fullPatchPath, string patchPath) : ITorrentFileSource
{
    /// <inheritdoc/>
    public string TorrentName { get; private set; } = "ffxiv";

    /// <inheritdoc/>
    public IEnumerable<FileMapping> Files { get; private set; } = [new(fullPatchPath, patchPath, new FileInfo(fullPatchPath).Length)];
}
