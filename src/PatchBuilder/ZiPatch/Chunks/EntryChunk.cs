// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace Rapture.PatchBuilder.ZiPatch.Chunks;

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
    [SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "This is for file hashing.")]
    public EntryChunk(string entryPath)
    {
        _entryPath = entryPath;
        _entryData = new();

        using var fileStream = File.OpenRead(Path.Combine(GetGameInstallPath(), entryPath));
        using var sha1 = SHA1.Create();

        fileStream.CopyTo(_entryData);
        fileStream.Position = 0;
        _oldEntryHash = sha1.ComputeHash(fileStream);
        _oldEntrySize = (uint)fileStream.Length;

        _entryData.Position = 0;
    }

    /// <inheritdoc/>
    [SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "This is for file hashing.")]
    public override void Write(PatchWriter writer)
    {
        using var sha1 = SHA1.Create();
        var newEntryHash = sha1.ComputeHash(_entryData);
        _entryData.Position = 0;

        writer.Write((uint)0); // Entry Size
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

    private static string GetGameInstallPath()
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{F2C4E6E0-EB78-4824-A212-6DF6AF0E8E82}")
            ?? throw new InvalidOperationException("Game is not installed.");

        if (key.GetValue("InstallLocation") is not string installLocation ||
            key.GetValue("DisplayName") is not string displayName)
        {
            throw new InvalidOperationException("Game is not installed.");
        }

        return Path.Combine(installLocation, displayName);
    }
}
