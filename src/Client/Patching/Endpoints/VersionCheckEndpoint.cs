// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.HttpResults;
using Rapture.Client.Patching.Results;
using Rapture.Client.Patching.Services;

namespace Rapture.Client.Patching.Endpoints;

/// <summary>
/// Provides configuration for the FFXIV patch version check API endpoint routing.
/// </summary>
public class VersionCheckEndpoint
{
    /// <summary>
    /// Configures the endpoint routing for the FFXIV patch version check API.
    /// </summary>
    /// <param name="builder">The endpoint route builder used to map routes for handling HTTP requests. Cannot be null.</param>
    public static void Configure(IEndpointRouteBuilder builder)
    {
        builder.MapGet("patch/vercheck/ffxiv/{platform}/{channel}/{type}/{version}", Handle);
    }

    private static Results<UpdateInfo, UpToDate, NotFound> Handle(PatchService patchService, string platform, string channel, string type, string version)
    {
        var currentPatch = patchService.GetPatch(platform, channel, type, version);

        if (currentPatch == null)
        {
            return TypedResults.NotFound();
        }

        var latestVersion = patchService.GetLatestVersion(platform, channel, type);

        if (version == latestVersion.Version)
        {
            return PatchResults.UpToDate(latestVersion);
        }

        var updateVersions = patchService.GetUpdateVersions(platform, channel, type, currentPatch);

        return PatchResults.UpdateInfo(updateVersions);
    }
}
