// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent;
using MonoTorrent.Client;

namespace Rapture.Client.Patching.Services;

/// <summary>
/// 
/// </summary>
/// <param name="clientEngine"></param>
/// <param name="hostEnvironment"></param>
public class TorrentService(ClientEngine clientEngine, IWebHostEnvironment hostEnvironment) : BackgroundService
{
    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dataPath = Path.Combine(hostEnvironment.WebRootPath, "patchdata", "ffxiv");

        var startTasks = Directory.GetFiles(dataPath, "*.torrent", SearchOption.AllDirectories)
            .Select(file =>
            {
                return Task.Run(async () =>
                {
                    var torrent = await Torrent.LoadAsync(file);
                    var manager = await clientEngine.AddAsync(torrent, dataPath);

                    await manager.StartAsync();
                });
            });

        await Task.WhenAll(startTasks);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(60000, stoppingToken);
        }

        var stopTasks = clientEngine.Torrents
            .Select(manager => manager.StopAsync());

        await Task.WhenAll(stopTasks);
    }
}
