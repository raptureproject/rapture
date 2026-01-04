// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using MonoTorrent;
using Rapture.Client.Patching.Logging;
using Rapture.Client.Patching.Results;

namespace Rapture.Client.Patching.Endpoints;

/// <summary>
/// Provides extension methods for configuring the application's endpoint routing to include the announce endpoint.
/// </summary>
public class AnnounceEndpoint
{
    /// <summary>
    /// Configures the application's endpoint routing to include the announce endpoint.
    /// </summary>
    /// <param name="builder">The endpoint route builder used to configure request routing for the application. Cannot be null.</param>
    public static void Configure(IEndpointRouteBuilder builder)
    {
        builder.MapGet("announce", Handle);
    }

    private static Announce Handle(ILogger<AnnounceEndpoint> logger, [FromQuery(Name = "info_hash")] string infoHashEncoded, [FromQuery(Name = "event")] string eventName)
    {
        var infoHash = InfoHash.UrlDecode(infoHashEncoded);

        if (eventName == "started")
        {
            PatchingLogger.LogClientAnnouncedStarted(logger, infoHash.ToHex());
        }
        else if (eventName == "stopped")
        {
            PatchingLogger.LogClientAnnouncedStopped(logger, infoHash.ToHex());
        }
        else if (eventName == "completed")
        {
            PatchingLogger.LogClientAnnouncedCompleted(logger, infoHash.ToHex());
        }

        return PatchResults.Announce();
    }
}
