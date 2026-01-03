// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

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

    private static Announce Handle()
    {
        return PatchResults.Announce("patch");
    }
}
