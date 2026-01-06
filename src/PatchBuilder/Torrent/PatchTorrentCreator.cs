// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent;

namespace Rapture.PatchBuilder.Torrent;

/// <summary>
/// Provides functionality for creating patch torrent files using the V1 torrent format.
/// </summary>
public class PatchTorrentCreator : TorrentCreator
{
    /// <summary>
    /// Initializes a new instance of the PatchTorrentCreator class with default settings for torrent creation using the V1 format.
    /// </summary>
    public PatchTorrentCreator() : base(TorrentType.V1Only)
    {
        Announce = "http://127.0.0.1:54997/announce";
        PieceLength = 262144;
    }

    /// <summary>
    /// Saves a patch torrent file using the specified source and destination paths.
    /// </summary>
    /// <param name="torrentPath">The file path where the patch torrent will be saved. Cannot be null or empty.</param>
    /// <param name="fullPatchPath">The file path to the full patch source. Must refer to an existing file.</param>
    /// <param name="patchPath">The file path to the patch source. Must refer to an existing file.</param>
    public void Save(string torrentPath, string fullPatchPath, string patchPath)
    {
        Create(new PatchTorrentSource(fullPatchPath, patchPath), torrentPath);
    }
}
