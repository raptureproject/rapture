// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using MonoTorrent;
using MonoTorrent.Client;
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

    private static Announce Handle(
        ILogger<AnnounceEndpoint> logger,
        ClientEngine clientEngine,
        [FromQuery(Name = "info_hash")] string infoHashEncoded,
        [FromQuery(Name = "peer_id")] string peerIdString,
        [FromQuery(Name = "event")] string eventName)
    {
        if (eventName == "completed")
        {
            var clientHash = InfoHash.UrlDecode(infoHashEncoded).Span[16..].ToArray();

            var torrent = clientEngine.Torrents
                .First(t =>
                {
                    var torrentHash = t.InfoHashes.V1OrV2.Span[16..];

                    return MemoryExtensions.SequenceEqual(clientHash, torrentHash);
                });

            PatchingLogger.LogClientCompletedDownloading(logger, torrent.Files.First().Path);
        }

        return PatchResults.Announce();
    }
}
