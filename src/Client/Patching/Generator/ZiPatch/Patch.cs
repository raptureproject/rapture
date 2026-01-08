// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Patching.Generator.Torrent;
using Rapture.Client.Patching.Generator.ZiPatch.Chunks;

namespace Rapture.Client.Patching.Generator.ZiPatch;

/// <summary>
/// Represents a patch that can be constructed, modified, and saved to disk, including support for generating an associated torrent file.
/// </summary>
public sealed class Patch : IDisposable
{
    private readonly string _patchPath;
    private readonly string _fullPatchPath;
    private readonly string _torrentPath;
    private readonly string _fullTorrentPath;
    private readonly PatchTorrentCreator _torrentCreator;
    private readonly List<Chunk> _chunks;

    /// <summary>
    /// Initializes a new instance of the Patch class with the specified date, revision, and patch type.
    /// </summary>
    /// <param name="year">The year component of the patch date. Must be a four-digit year.</param>
    /// <param name="month">The month component of the patch date, ranging from 1 to 12.</param>
    /// <param name="day">The day component of the patch date, ranging from 1 to 31 depending on the month.</param>
    /// <param name="revision">The revision number of the patch. Must be a non-negative integer.</param>
    /// <param name="type">The type of patch to create. Determines the patch depot used for file paths.</param>
    public Patch(int year, int month, int day, int revision, PatchType type)
    {
        var patchRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "src", "client", "wwwroot", "patchdata", "ffxiv");
        var patchDepot = type == PatchType.Boot ? "2d2a390f" : "48eca647";
        var patchName = $"D{year:0000}.{month:00}.{day:00}.{revision:0000}";

        _patchPath = Path.Combine(patchDepot, "patch", $"{patchName}.patch");
        _fullPatchPath = Path.Combine(patchRoot, _patchPath);
        _torrentPath = Path.Combine(patchDepot, "metainfo", $"{patchName}.torrent");
        _fullTorrentPath = Path.Combine(patchRoot, _torrentPath);
        _torrentCreator = new();
        _chunks = [];

        _chunks.Add(new FileHeaderChunk(_chunks));
        _chunks.Add(new ApplyChunk(1));
        _chunks.Add(new ApplyChunk(2));
    }

    /// <summary>
    /// Adds a file to the patch for the specified file path.
    /// </summary>
    /// <param name="filePath">The path to the file to add. Cannot be null or empty.</param>
    /// <returns>An <see cref="EntryChunk"/> representing the added file.</returns>
    public EntryChunk AddFile(string filePath)
    {
        var entryChunk = new EntryChunk(filePath);
        _chunks.Add(entryChunk);
        return entryChunk;
    }

    /// <summary>
    /// Saves the current patch data to disk and generates the associated torrent file.
    /// </summary>
    public void Save()
    {
        using var fileStream = File.Create(_fullPatchPath);
        using var patchWriter = new PatchWriter(fileStream);

        patchWriter.Write([0x91, 0x5A, 0x49, 0x50, 0x41, 0x54, 0x43, 0x48, 0x0D, 0x0A, 0x1A, 0x0A]);

        foreach (var chunk in _chunks)
        {
            chunk.Write(patchWriter);
        }

        patchWriter.Close();
        fileStream.Close();

        _torrentCreator.Save(_fullTorrentPath, _fullPatchPath, _patchPath);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        foreach (var chunk in _chunks)
        {
            chunk.Dispose();
        }

        _chunks.Clear();

        GC.SuppressFinalize(this);
    }
}
