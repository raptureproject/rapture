// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using System.Text;

namespace Rapture.PatchBuilder.ZiPatch.Chunks;

/// <summary>
/// Represents the file header chunk in a patch file, containing metadata and version information required for interpreting the patch contents.
/// </summary>
/// <param name="chunks">The collection of child chunks associated with this patch file.</param>
public class FileHeaderChunk(List<Chunk> chunks) : Chunk
{
    /// <inheritdoc/>
    public override void Write(PatchWriter writer)
    {
        writer.Write((uint)20); // File header Size
        writer.StartCrc();
        writer.Write(Encoding.ASCII.GetBytes("FHDR")); // FHDR
        writer.Write([0x00, 0x00, 0x02, 0x00]); // Version 2.0
        writer.Write(Encoding.ASCII.GetBytes("HIST")); // HIST patch
        writer.Write((uint)chunks.Count(c => c is EntryChunk)); // Entry count
        writer.Write((uint)0); // Added directory count
        writer.Write((uint)0); // Deleted directory count
        writer.WriteCrc();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
