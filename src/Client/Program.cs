// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Core;
using Rapture.Client.Launcher;
using Rapture.Client.Patching;
using Rapture.Client.Shell;

var builder = WebApplication.CreateSlimBuilder();

builder.ConfigureCore()
    .ConfigureShell()
    .ConfigurePatching()
    .ConfigureLauncher();

var app = builder.Build();

app.UseCore()
    .UseShell()
    .UsePatching();

app.Run();
