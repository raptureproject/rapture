// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.Metadata;
using Rapture.Client.Patching.Models;
using System.Reflection;

namespace Rapture.Client.Patching.Results;

/// <summary>
/// Represents an HTTP 204 No Content result indicating that the client is already up to date, including information about the latest available patch.
/// </summary>
/// <param name="info">The patch information representing the latest available version.</param>
public class UpToDate(PatchInfo info) : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<PatchInfo>
{
    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status204NoContent"/>
    /// </summary>
    public int? StatusCode => 204;

    /// <summary>
    /// Gets the latest patch info.
    /// </summary>
    public PatchInfo? Value { get; } = info;

    object? IValueHttpResult.Value => Value;

    PatchInfo? IValueHttpResult<PatchInfo>.Value => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task ExecuteAsync(HttpContext httpContext)
    {
        if (Value == null)
        {
            throw new InvalidOperationException("Patch data cannot be null!");
        }

        httpContext.Response.StatusCode = (int)StatusCode!;

        httpContext.Response.Headers["Content-Location"] = $"ffxiv/{Value.RepositoryHash}/vercheck.dat";
        httpContext.Response.Headers["X-Repository"] = $"ffxiv/{Value.Platform}/{Value.Channel}/{Value.Type}";
        httpContext.Response.Headers["X-Patch-Module"] = "ZiPatch";
        httpContext.Response.Headers["X-Protocol"] = "torrent";
        httpContext.Response.Headers["X-Info-Url"] = "http://example.com";
        httpContext.Response.Headers["X-Latest-Version"] = Value.Version;

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status204NoContent, typeof(void)));
    }
}
