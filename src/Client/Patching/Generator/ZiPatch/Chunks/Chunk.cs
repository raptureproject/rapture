// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

namespace Rapture.Client.Patching.Generator.ZiPatch.Chunks;

/// <summary>
/// Represents an abstract data chunk that can be written to a binary stream.
/// </summary>
public abstract class Chunk : IDisposable
{
    /// <summary>
    /// Writes the chunk's data to the specified binary stream using the provided writer.
    /// </summary>
    /// <param name="writer">The <see cref="PatchWriter"/> used to write the chunk's data to the output stream. Cannot be null.</param>
    public abstract void Write(PatchWriter writer);

    /// <inheritdoc/>
    public abstract void Dispose();
}
