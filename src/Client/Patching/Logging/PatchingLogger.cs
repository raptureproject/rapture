// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

namespace Rapture.Client.Patching.Logging;

/// <summary>
/// Provides strongly-typed logging methods for client version checking and update events within the patching process.
/// </summary>
public static partial class PatchingLogger
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Warning, Message = "Client requested {Type} version {Version}, but it was not found.")]
    public static partial void LogVersionNotFound(ILogger logger, string type, string version);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Client {Type} version {Version} is up to date.")]
    public static partial void LogClientUpToDate(ILogger logger, string type, string version);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Client {Type} version {CurrentVersion} needs update to {LatestVersion}.")]
    public static partial void LogClientNeedsUpdate(ILogger logger, string type, string currentVersion, string latestVersion);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Client announced started downloading {InfoHash}.")]
    public static partial void LogClientAnnouncedStarted(ILogger logger, string infoHash);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Client announced stopped downloading {InfoHash}.")]
    public static partial void LogClientAnnouncedStopped(ILogger logger, string infoHash);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Client announced completed downloading {InfoHash}.")]
    public static partial void LogClientAnnouncedCompleted(ILogger logger, string infoHash);
}
