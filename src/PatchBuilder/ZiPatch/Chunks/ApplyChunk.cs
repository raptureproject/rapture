// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using System.Text;

namespace Rapture.PatchBuilder.ZiPatch.Chunks;

/// <summary>
/// Represents a patch chunk that applies an option to the target using the 'APLY' format.
/// </summary>
/// <param name="option">The option value to be applied in the patch chunk. The meaning of this value depends on the patch specification.</param>
public class ApplyChunk(uint option) : Chunk
{
    /// <inheritdoc/>
    public override void Write(PatchWriter writer)
    {
        writer.Write((uint)12); // Apply Size
        writer.StartCrc();
        writer.Write(Encoding.ASCII.GetBytes("APLY")); // APLY
        writer.Write(option); // Option
        writer.Write((uint)4); // Unknown
        writer.Write((uint)1); // Unknown
        writer.WriteCrc();
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
