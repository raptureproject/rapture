// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Rapture.Client.Core.Utilities;

/// <summary>
/// Provides utility methods for computing cryptographic hashes of files.
/// </summary>
public static class HashingUtilities
{
    /// <summary>
    /// Computes the SHA-1 hash of the contents of the specified file and returns it as a hexadecimal string.
    /// </summary>
    /// <param name="path">The path to the file to hash. The file must exist and be accessible for reading.</param>
    /// <returns>A hexadecimal string representation of the SHA-1 hash of the file's contents.</returns>
    public static byte[] SHA1HashFile(string path)
    {
        using var file = new FileStream(path, FileMode.Open, FileAccess.Read);
        return SHA1HashStream(file);
    }

    /// <summary>
    /// Computes the SHA-1 hash value for the data contained in the specified stream.
    /// </summary>
    /// <param name="stream">The input stream containing the data to be hashed.</param>
    /// <returns>A byte array containing the SHA-1 hash of the stream's data.</returns>
    [SuppressMessage("Security", "CA5350:Do Not Use Weak Cryptographic Algorithms", Justification = "This is for file hashing.")]
    public static byte[] SHA1HashStream(Stream stream)
    {
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(stream);

        return hash;
    }
}
