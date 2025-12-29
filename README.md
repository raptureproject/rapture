# Rapture
[![Build](https://github.com/raptureproject/rapture/actions/workflows/build-code.yml/badge.svg?branch=main&event=push)](https://github.com/raptureproject/rapture/actions/workflows/build-code.yml)
[![Lint](https://github.com/raptureproject/rapture/actions/workflows/lint-code.yml/badge.svg?branch=main&event=push)](https://github.com/raptureproject/rapture/actions/workflows/lint-code.yml)
[![Scan](https://github.com/raptureproject/rapture/actions/workflows/scan-code.yml/badge.svg?branch=main&event=push)](https://github.com/raptureproject/rapture/actions/workflows/scan-code.yml)

## What is Rapture?
Rapture is a Final Fantasy XIV 1.23b server emulator.

## What is in this repo?
This repository contains all source code for Rapture. Everything needed to develop or deploy Rapture is self contained in this repository.

## Getting started
To start developing Rapture you need just a few things:

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Aspire CLI](https://aspire.dev/get-started/install-cli/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/), [Podman Desktop](https://podman-desktop.io/), or [Rancher Desktop](https://rancherdesktop.io/)

## Run Rapture
Once you have all the dependencies listed above simply execute `aspire run` to start Rapture. You can access the following services:

- Aspire Dashboard: https://aspire.dev.localhost:8440
- Rapture: https://rapture.dev.localhost:8443

The Aspire dashboard may prompt for additional values in order to start Rapture, filling these out will start the service.
