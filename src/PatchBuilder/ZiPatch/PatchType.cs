// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

namespace Rapture.PatchBuilder.ZiPatch;

/// <summary>
/// Specifies the type of patch to apply, such as for boot or game components.
/// </summary>
public enum PatchType
{
    /// <summary>
    /// Represents a patch for boot.
    /// </summary>
    Boot,

    /// <summary>
    /// Represents a patch for the game.
    /// </summary>
    Game
}
