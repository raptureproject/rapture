// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Launcher.Services;

namespace Rapture.Client.Launcher;

/// <summary>
/// Provides extension methods for configuring launcher-related services in an application host.
/// </summary>
/// <remarks>This static class contains methods that extend <see cref="IHostApplicationBuilder"/> to simplify the
/// registration of launcher services. These methods are intended to be used during application startup to ensure that
/// the launcher service is properly integrated as a hosted service.</remarks>
public static class LauncherExtensions
{
    /// <summary>
    /// Configures the application to use the launcher service by adding it as a hosted service.
    /// </summary>
    /// <param name="builder">The application builder used to configure services and middleware for the host.</param>
    /// <returns>The same <paramref name="builder"/> instance, enabling method chaining.</returns>
    public static IHostApplicationBuilder ConfigureLauncher(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<LauncherService>();

        return builder;
    }
}
