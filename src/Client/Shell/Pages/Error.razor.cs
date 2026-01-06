// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace Rapture.Client.Shell.Pages;

/// <summary>
/// Represents the error page component that displays error details for the current HTTP request.
/// </summary>
#pragma warning disable CA1716 // Identifiers should not match keywords
public partial class Error
#pragma warning restore CA1716 // Identifiers should not match keywords
{
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }

    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }
}
