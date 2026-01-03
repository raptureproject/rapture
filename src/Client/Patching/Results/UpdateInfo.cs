// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Metadata;
using Rapture.Client.Patching.Models;
using System.Reflection;

namespace Rapture.Client.Patching.Results;

/// <summary>
/// Represents the result of an update information request, providing a collection of patch details in a multipart HTTP response.
/// </summary>
/// <param name="updateInfo">The collection of patch information to include in the response. Cannot be null.</param>
public class UpdateInfo(IEnumerable<PatchInfo> updateInfo) : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<IEnumerable<PatchInfo>>
{
    /// <summary>
    /// Gets the HTTP status code: <see cref="StatusCodes.Status200OK"/>
    /// </summary>
    public int? StatusCode => 200;

    /// <summary>
    /// Gets the update info.
    /// </summary>
    public IEnumerable<PatchInfo>? Value { get; } = updateInfo;

    object? IValueHttpResult.Value => Value;

    /// <inheritdoc/>
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (Value == null)
        {
            throw new InvalidOperationException("Update info cannot be null!");
        }

        httpContext.Features.Get<IHttpBodyControlFeature>()!.AllowSynchronousIO = true;

        httpContext.Response.StatusCode = (int)StatusCode!;
        httpContext.Response.ContentType = "multipart/mixed; boundary=477D80B1_38BC_41d4_8B48_5273ADB89CAC";

        var latestInfo = Value.Last();
        var hostEnvironment = httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
        var basePath = Path.Combine(hostEnvironment.WebRootPath, "patchdata", "ffxiv");

        httpContext.Response.Headers["Connection"] = "keep-alive";
        httpContext.Response.Headers["Content-Location"] = $"ffxiv/{latestInfo.RepositoryHash}/vercheck.dat";
        httpContext.Response.Headers["X-Repository"] = $"ffxiv/{latestInfo.Platform}/{latestInfo.Channel}/{latestInfo.Type}";
        httpContext.Response.Headers["X-Patch-Module"] = "ZiPatch";
        httpContext.Response.Headers["X-Protocol"] = "torrent";
        httpContext.Response.Headers["X-Info-Url"] = "http://example.com";
        httpContext.Response.Headers["X-Latest-Version"] = latestInfo.Version;

        using var writer = new StreamWriter(httpContext.Response.Body, leaveOpen: true);

        foreach (var updateVersion in Value)
        {
            using var reader = File.OpenRead(Path.Combine(basePath, updateVersion.RepositoryHash, "metainfo", $"D{updateVersion.Version}.torrent"));

            await writer.WriteAsync("--477D80B1_38BC_41d4_8B48_5273ADB89CAC\r\n");
            await writer.WriteAsync("Content-Type: application/octet-stream\r\n");
            await writer.WriteAsync($"Content-Location: ffxiv/{updateVersion.RepositoryHash}/metainfo/D{updateVersion.Version}.torrent\r\n");
            await writer.WriteAsync($"X-Patch-Length: {updateVersion.FileSize}\r\n");
            await writer.WriteAsync("X-Signature: jqxmt9WQH1aXptNju6CmCdztFdaKbyOAVjdGw_DJvRiBJhnQL6UlDUcqxg2DeiIKhVzkjUm3hFXOVUFjygxCoPUmCwnbCaryNqVk_oTk_aZE4HGWNOEcAdBwf0Gb2SzwAtk69zs_5dLAtZ0mPpMuxWJiaNSvWjEmQ925BFwd7Vk=\r\n");
            await writer.WriteAsync("\r\n");
            await writer.FlushAsync();
            await reader.CopyToAsync(writer.BaseStream);
        }

        await writer.WriteAsync("--477D80B1_38BC_41d4_8B48_5273ADB89CAC--\r\n\r\n");
    }

    /// <inheritdoc/>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(void)));
    }
}
