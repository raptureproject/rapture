// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

namespace Rapture.Client.Patching.Models;

/// <summary>
/// Represents metadata about a game patch, including platform, release channel, type, version, build time, file size, and repository hash.
/// </summary>
public class PatchInfo
{
    /// <summary>
    /// Gets the name of the platform associated with this instance.
    /// </summary>
    public required string Platform { get; init; }

    /// <summary>
    /// Gets the name of the channel associated with this instance.
    /// </summary>
    public required string Channel { get; init; }

    /// <summary>
    /// Gets the type identifier for the patch.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets the version identifier for the current instance.
    /// </summary>
    public required string Version { get; init; }

    /// <summary>
    /// Gets the date on which the build was created.
    /// </summary>
    public required DateOnly BuildTime { get; init; }

    /// <summary>
    /// Gets the size of the file, in bytes.
    /// </summary>
    public required ulong FileSize { get; init; }

    /// <summary>
    /// Gets the unique hash of the repository.
    /// </summary>
    public required string RepositoryHash { get; init; }
}
