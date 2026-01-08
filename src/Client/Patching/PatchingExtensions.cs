// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent.Client;
using Rapture.Client.Patching.Endpoints;
using Rapture.Client.Patching.Generator;
using Rapture.Client.Patching.Repositories;
using Rapture.Client.Patching.Services;

namespace Rapture.Client.Patching;

/// <summary>
/// Provides extension methods for configuring and enabling patching functionality in an application host or web application.
/// </summary>
public static class PatchingExtensions
{
    /// <summary>
    /// Configures patching services and registers required dependencies for patch management in the application.
    /// </summary>
    /// <param name="builder">The application builder used to configure services and middleware for the host.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> instance so that additional configuration calls can be chained.</returns>
    public static IHostApplicationBuilder ConfigurePatching(this IHostApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool>("GeneratePatches"))
        {
            PatchGenerator.GenerateAllPatches();
            Environment.Exit(0);
        }

        builder.Services.AddSingleton(services =>
        {
            var webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();

            var builder = new EngineSettingsBuilder()
            {
                CacheDirectory = Path.Combine(webHostEnvironment.WebRootPath, "patchdata", "cache")
            };

            return builder.ToSettings();
        });

        builder.Services.AddSingleton(services =>
        {
            return new ClientEngine(services.GetRequiredService<EngineSettings>());
        });

        builder.Services.AddHostedService<TorrentService>();
        builder.Services.AddSingleton<PatchRepository>();
        builder.Services.AddScoped<PatchService>();

        return builder;
    }

    /// <summary>
    /// Enables patching support by configuring the patching endpoint for the specified web application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure for patching support.</param>
    /// <returns>The same <see cref="WebApplication"/> instance, to enable method chaining.</returns>
    public static WebApplication UsePatching(this WebApplication app)
    {
        VersionCheckEndpoint.Configure(app);
        AnnounceEndpoint.Configure(app);

        return app;
    }
}
