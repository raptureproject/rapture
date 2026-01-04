// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Mvc;
using MonoTorrent;
using MonoTorrent.BEncoding;
using MonoTorrent.Client;
using Rapture.Client.Patching.Logging;
using Rapture.Client.Patching.Results;
using Rapture.Common.Cryptography;
using System.Security.Cryptography;

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
            var infoHash = InfoHash.UrlDecode(infoHashEncoded);
            var peerId = new BEncodedString(peerIdString);

            using var blowfish = Blowfish.Create();
            blowfish.Key = peerId.Span.ToArray();

            var torrent = clientEngine.Torrents
                .First(t =>
                {
                    var encHash = t.InfoHashes.V1OrV2.Span.ToArray();
                    blowfish.EncryptEcb(t.InfoHashes.V1OrV2.Span[..16], encHash, PaddingMode.None);

                    return MemoryExtensions.SequenceEqual(infoHash.Span, encHash.AsSpan());
                });

            PatchingLogger.LogClientCompletedDownloading(logger, torrent.Files.First().Path);
        }

        return PatchResults.Announce();
    }
}
