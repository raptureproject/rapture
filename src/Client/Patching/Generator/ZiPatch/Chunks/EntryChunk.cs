// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Core.Utilities;
using System.Text;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Rapture.Client.Patching.Generator.ZiPatch.Chunks;

/// <summary>
/// Represents a chunk that encapsulates a file entry, including its path, data, hash, and size information, for use in patching operations.
/// </summary>
public class EntryChunk : Chunk, IDisposable
{
    private readonly string _entryPath;
    private readonly MemoryStream _entryData;
    private readonly byte[] _oldEntryHash;
    private readonly uint _oldEntrySize;

    /// <summary>
    /// Initializes a new instance of the EntryChunk class by loading the specified file and computing its hash and size.
    /// </summary>
    /// <param name="entryPath">The relative path to the file to be loaded. Must not be null or empty.</param>
    public EntryChunk(string entryPath)
    {
        _entryPath = entryPath;
        _entryData = new();

        var gamePath = GameUtilities.GetGameInstallPath();

        if (gamePath is null)
        {
            PInvoke.MessageBox(HWND.Null, "A Final Fantasy XIV 1.0 Install Was Not Found!", "Final Fantasy XIV Not Installed", MESSAGEBOX_STYLE.MB_ICONERROR);
            Environment.Exit(1);
        }

        using var fileStream = File.OpenRead(Path.Combine(gamePath, entryPath));

        fileStream.CopyTo(_entryData);
        fileStream.Position = 0;
        _oldEntryHash = HashingUtilities.SHA1HashStream(fileStream);
        _oldEntrySize = (uint)fileStream.Length;

        _entryData.Position = 0;
    }

    /// <inheritdoc/>
    public override void Write(PatchWriter writer)
    {
        var newEntryHash = HashingUtilities.SHA1HashStream(_entryData);
        var entrySize = 68 + _entryPath.Length + _entryData.Length;
        _entryData.Position = 0;

        writer.Write((uint)entrySize); // Entry Size
        writer.StartCrc();
        writer.Write(Encoding.ASCII.GetBytes("ETRY")); // ETRY
        writer.Write((uint)_entryPath.Length); // Entry Path Length
        writer.Write(Encoding.ASCII.GetBytes(_entryPath)); // Entry Path
        writer.Write((uint)1); // Entry file count
        writer.Write([0x4D, 0x00, 0x00, 0x00]); // Entry mode
        writer.Write(_oldEntryHash); // Old Entry Hash
        writer.Write(newEntryHash); // New Entry Hash
        writer.Write([0x4E, 0x00, 0x00, 0x00]); // Compression mode
        writer.Write((uint)_entryData.Length); // Data size
        writer.Write(_oldEntrySize); // Old entry size
        writer.Write((uint)_entryData.Length); // New entry size
        writer.Write(_entryData.ToArray()); // Entry data
        writer.WriteCrc();
    }

    /// <summary>
    /// Writes the specified patch data to the underlying data stream at the given address.
    /// </summary>
    /// <param name="address">The zero-based position within the data stream at which to begin writing the patch data.</param>
    /// <param name="patchData">The byte array containing the patch data to write. Cannot be null.</param>
    public void ApplyPatch(uint address, byte[] patchData)
    {
        _entryData.Position = address;
        _entryData.Write(patchData, 0, patchData.Length);
        _entryData.Position = 0;
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        _entryData.Dispose();

        GC.SuppressFinalize(this);
    }
}
