// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Rapture_Client>("client");

builder.Build().Run();
