// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Rapture.Client.Core;

/// <summary>
/// Provides extension methods for configuring core services and middleware in a web application, including URLs,
/// telemetry, health checks, service discovery, and HTTP client support.
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    /// Configures core services and middleware for the application, including URLs, telemetry, health checks, service discovery, and HTTP client support.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance to configure. Cannot be null.</param>
    /// <returns>The same WebApplicationBuilder instance, configured with core services and middleware.</returns>
    public static WebApplicationBuilder ConfigureCore(this WebApplicationBuilder builder)
    {
        return builder.ConfigureUrls()
            .ConfigureTelemetry()
            .ConfigureHealthChecks()
            .ConfigureServiceDiscovery()
            .ConfigureHttpClient();
    }

    /// <summary>
    /// Adds core middleware components to the specified web application.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure with core middleware.</param>
    /// <returns>The configured <see cref="WebApplication"/> instance for chaining additional middleware registrations.</returns>
    public static WebApplication UseCore(this WebApplication app)
    {
        return app.UseHealthChecks();
    }

    private static WebApplicationBuilder ConfigureUrls(this WebApplicationBuilder builder)
    {
        builder.WebHost.UseUrls(
            "http://127.0.0.1:54996", // Version Check
            "http://127.0.0.1:54997" // Patch Tracker
        );

        return builder;
    }

    private static WebApplicationBuilder ConfigureTelemetry(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(tracing =>
                        tracing.Filter = context =>
                            !context.Request.Path.StartsWithSegments("/health")
                            && !context.Request.Path.StartsWithSegments("/alive")
                    )
                    .AddHttpClientInstrumentation();
            });

        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            builder.Services.AddOpenTelemetry()
                .UseOtlpExporter();
        }

        return builder;
    }

    private static WebApplicationBuilder ConfigureHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    private static WebApplicationBuilder ConfigureServiceDiscovery(this WebApplicationBuilder builder)
    {
        builder.Services.AddServiceDiscovery();

        return builder;
    }

    private static WebApplicationBuilder ConfigureHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });

        return builder;
    }

    private static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health");

        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });

        return app;
    }
}
