// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Shell.Components;

namespace Rapture.Client.Shell;

/// <summary>
/// Provides extension methods for configuring Razor component support and standard shell middleware in ASP.NET Core applications.
/// </summary>
public static class ShellExtensions
{
    /// <summary>
    /// Configures the host builder to support Razor components in the application shell.
    /// </summary>
    /// <param name="builder">The host application builder to configure. Cannot be null.</param>
    /// <returns>The same <see cref="IHostApplicationBuilder"/> instance for chaining further configuration.</returns>
    public static IHostApplicationBuilder ConfigureShell(this IHostApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents();

        return builder;
    }

    /// <summary>
    /// Configures the specified web application to use standard shell middleware, including error handling, status code pages, and Razor components.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <returns>The configured <see cref="WebApplication"/> instance for chaining.</returns>
    public static WebApplication UseShell(this WebApplication app)
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

        app.UseAntiforgery();

        app.MapRazorComponents<App>();

        return app;
    }
}
