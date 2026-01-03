// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Patching.Models;
using Rapture.Client.Patching.Repositories;

namespace Rapture.Client.Patching.Services;

/// <summary>
/// Provides methods for retrieving and managing patch information for different platforms, release channels, and patch types.
/// </summary>
/// <param name="patchRepository">The repository used to access patch data. Cannot be null.</param>
public class PatchService(PatchRepository patchRepository)
{
    /// <summary>
    /// Retrieves patch information for the specified platform, channel, type, and version.
    /// </summary>
    /// <param name="platform">The platform for which to retrieve patch versions.</param>
    /// <param name="channel">The release channel to filter patch versions by.</param>
    /// <param name="type">The type of patch to retrieve.</param>
    /// <param name="version">The version identifier of the patch to retrieve. This value is case-sensitive and cannot be null.</param>
    /// <returns>A <see cref="PatchInfo"/> object containing the patch details if a matching patch is found; otherwise, <see langword="null"/>.</returns>
    public PatchInfo? GetPatch(string platform, string channel, string type, string version)
    {
        return patchRepository.PatchInfo
            .Where(v => v.Platform == platform)
            .Where(v => v.Channel == channel)
            .Where(v => v.Type == type)
            .FirstOrDefault(v => v.Version == version);
    }

    /// <summary>
    /// Retrieves the most recent patch information for the specified platform, channel, and type.
    /// </summary>
    /// <param name="platform">The platform for which to retrieve patch versions.</param>
    /// <param name="channel">The release channel to filter patch versions by.</param>
    /// <param name="type">The type of patch to retrieve.</param>
    /// <returns>A <see cref="PatchInfo"/> object representing the latest available patch for the specified platform, channel, and type.</returns>
    public PatchInfo GetLatestVersion(string platform, string channel, string type)
    {
        return patchRepository.PatchInfo
            .Where(v => v.Platform == platform)
            .Where(v => v.Channel == channel)
            .Where(v => v.Type == type)
            .OrderBy(v => v.BuildTime)
            .Last();
    }

    /// <summary>
    /// Retrieves a list of available patch versions newer than the specified current version for the given platform, channel, and type.
    /// </summary>
    /// <param name="platform">The platform for which to retrieve patch versions.</param>
    /// <param name="channel">The release channel to filter patch versions by.</param>
    /// <param name="type">The type of patch to retrieve.</param>
    /// <param name="currentVersion">The current patch version.</param>
    /// <returns>A list of PatchInfo objects representing patch versions that are newer than the specified current version and match the given platform, channel, and type.</returns>
    public List<PatchInfo> GetUpdateVersions(string platform, string channel, string type, PatchInfo currentVersion)
    {
        return [.. patchRepository.PatchInfo
            .Where(v => v.Platform == platform)
            .Where(v => v.Channel == channel)
            .Where(v => v.Type == type)
            .Where(v => v.BuildTime > currentVersion.BuildTime)
            .OrderBy(v => v.BuildTime)];
    }
}
