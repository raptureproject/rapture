// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.Metadata;
using MonoTorrent.BEncoding;
using System.Net;
using System.Reflection;

namespace Rapture.Client.Patching.Results;

/// <summary>
/// Creates a <see cref="Announce"/> result type.
/// </summary>
/// <param name="serviceName">The service name that contains the IPs of peers.</param>
public class Announce(string serviceName) : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<string>
{
    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status200OK"/>
    /// </summary>
    public int? StatusCode => 200;

    /// <summary>
    /// Gets the service name.
    /// </summary>
    public string? Value { get; } = serviceName;

    object? IValueHttpResult.Value => Value;

    /// <inheritdoc/>
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (Value == null)
        {
            throw new InvalidOperationException("Service name cannot be null!");
        }

        httpContext.Response.StatusCode = (int)StatusCode!;
        httpContext.Response.ContentType = "text/plain";

        httpContext.Response.Headers["Connection"] = "close";

        byte[] peers =
        [
            .. IPAddress.Loopback.GetAddressBytes(),
            .. BitConverter.GetBytes(54998)
        ];

        var response = new BEncodedDictionary
        {
            { new("tracker id"), new BEncodedString("SQ0001-DcPDIHCph") },
            { new("interval"), new BEncodedNumber(2700) },
            { new("min interval"), new BEncodedNumber(600) },
            { new("complete"), new BEncodedNumber(1) },
            { new("incomplete"), new BEncodedNumber(0) },
            { new("downloaded"), new BEncodedNumber(0) },
            { new("peers"), new BEncodedString(peers) }
        };

        await httpContext.Response.Body.WriteAsync(response.Encode());
    }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(void)));
    }
}
