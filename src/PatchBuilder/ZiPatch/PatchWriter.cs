// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using System.IO.Hashing;

namespace Rapture.PatchBuilder.ZiPatch;

/// <summary>
/// Provides functionality for writing patch data to a stream in binary format.
/// </summary>
public class PatchWriter(Stream output) : BinaryWriter(output)
{
    private readonly Crc32 _crc = new();

    /// <inheritdoc/>
    public override void Write(uint value)
    {
        var bytes = BitConverter.GetBytes(value).AsSpan();
        bytes.Reverse();
        _crc.Append(bytes);
        base.Write(bytes);
    }

    /// <inheritdoc/>
    public override void Write(byte[] buffer)
    {
        _crc.Append(buffer);
        base.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        _crc.Append(buffer);
        base.Write(buffer);
    }

    /// <summary>
    /// Initializes or resets the CRC calculation to its starting state.
    /// </summary>
    public void StartCrc()
    {
        _crc.Reset();
    }

    /// <summary>
    /// Writes the current CRC value to the underlying stream in big-endian byte order.
    /// </summary>
    public void WriteCrc()
    {
        var hash = _crc.GetCurrentHash().AsSpan();
        hash.Reverse();
        base.Write(hash);
    }
}
