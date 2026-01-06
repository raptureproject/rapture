// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using MonoTorrent;
using MonoTorrent.Client;
using Rapture.Client.Patching.Logging;

namespace Rapture.Client.Patching.Services;

/// <summary>
/// Provides a background service that manages the lifecycle of torrent downloads for patch data using the specified client engine.
/// </summary>
/// <param name="clientEngine">The torrent client engine used to manage and coordinate torrent downloads.</param>
/// <param name="hostEnvironment">The web host environment that provides access to the application's web root path for locating patch data.</param>
/// <param name="logger">The logger used to record informational and diagnostic messages related to torrent activity.</param>
public class TorrentService(ClientEngine clientEngine, IWebHostEnvironment hostEnvironment, ILogger<TorrentService> logger) : IHostedService
{
    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var dataPath = Path.Combine(hostEnvironment.WebRootPath, "patchdata", "ffxiv");

        var startTasks = Directory.GetFiles(dataPath, "*.torrent", SearchOption.AllDirectories)
            .Select(file =>
            {
                return Task.Run(async () =>
                {
                    var torrent = await Torrent.LoadAsync(file);
                    var manager = await clientEngine.AddAsync(torrent, dataPath);

                    manager.PeerConnected += OnPeerConnected;
                    manager.PeerDisconnected += OnPeerDisconnected;

                    await manager.StartAsync();
                });
            });

        return Task.WhenAll(startTasks);
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        var stopTasks = clientEngine.Torrents
            .Select(manager =>
            {
                return Task.Run(async () =>
                {
                    manager.PeerConnected -= OnPeerConnected;
                    manager.PeerDisconnected -= OnPeerDisconnected;

                    await manager.StopAsync();
                });
            });

        return Task.WhenAll(stopTasks);
    }

    private void OnPeerConnected(object? sender, PeerConnectedEventArgs e)
    {
        PatchingLogger.LogClientStartedDownloading(logger, e.TorrentManager.Files.First().Path);
    }

    private void OnPeerDisconnected(object? sender, PeerDisconnectedEventArgs e)
    {
        PatchingLogger.LogClientStoppedDownloading(logger, e.TorrentManager.Files.First().Path);
    }
}
