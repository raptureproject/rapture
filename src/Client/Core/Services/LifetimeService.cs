// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Rapture.Client.Core.Services;

/// <summary>
/// Provides a background service that monitors specific processes and requests application shutdown when none are running.
/// </summary>
public class LifetimeService : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            if (GetMonitoredProcessesCount() == 0)
            {
                await Task.Delay(500, stoppingToken);

                // Double check no processes are running.
                if (GetMonitoredProcessesCount() == 0)
                {
                    Environment.Exit(0);
                }
            }
        }
    }

    private static int GetMonitoredProcessesCount()
    {
        return Process.GetProcessesByName("ffxivboot")
            .Concat(Process.GetProcessesByName("ffxivupdater"))
            .Concat(Process.GetProcessesByName("ffxivlogin"))
            .Concat(Process.GetProcessesByName("ffxivgame"))
            .Count();
    }
}
