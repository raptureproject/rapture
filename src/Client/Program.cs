// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Core;
using Rapture.Client.Launcher;

var builder = WebApplication.CreateSlimBuilder();

builder.ConfigureCore()
    .ConfigureLauncher();

var app = builder.Build();

app.UseCore();

app.Run();
