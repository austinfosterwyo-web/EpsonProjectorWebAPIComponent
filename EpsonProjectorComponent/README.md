# Epson Projector Web Remote (Blazor Server)

## Overview

This project is a Blazor Server application that implements a web-based remote control for Epson projectors using Epson's published Web API.  
It demonstrates a clean, object-oriented approach to device control, API integration, and UI interaction in a pattern similar to AV control system drivers and modules.

The solution separates device logic, transport/authentication concerns, and presentation, allowing the same client workflow to run against either a real projector or a simulator.

## Why This Project Exists

This project demonstrates:

- API integration with a hardware device interface
- Object-oriented design and separation of concerns
- Driver-style architecture used in AV systems
- Real-world behavior like authentication, retries, and state handling

It is intentionally scoped to core remote-control functionality rather than full device fleet management.

## Features

- Power on / power off control
- Audio mute control
- Input source selection
- Connection configuration (IP address and credentials)
- Built-in simulator for development without physical hardware
- Logging and basic error handling
- Reusable drop-in Razor component (`ProjectorRemote`)

## Architecture

The solution is split into three logical layers:

### 1. Device / Client Layer (`Epson.Projector`)

Encapsulates projector behavior and exposes high-level methods:

- `PowerOnAsync()`
- `PowerOffAsync()`
- `SetMuteAsync(bool)`
- `SetSourceAsync(string)`

This layer has no UI logic.

### 2. Transport & Authentication Layer (`Epson.Projector.Transport`)

Responsible for communication details:

- HTTP request/response handling
- Authentication strategy selection (Digest for real devices, no-auth for simulator)
- Retry and timeout behavior

A transport interface supports swapping between real HTTP transport and simulated transport.

### 3. UI Layer (Blazor Server)

Provides a browser-based remote:

- Connection/configuration controls
- Remote command buttons
- Status and response feedback

The UI uses a service/client abstraction and does not issue raw HTTP commands.

## Import Into Another Project

The remote is now componentized and can be embedded in another Blazor Server app.

1. Reference this project from your host app (`ProjectReference` or packaged library).
2. Register services in `Program.cs`:

```csharp
builder.Services.AddEpsonProjectorRemote();
```

3. Add namespace import (in `_Imports.razor` or page):

```razor
@using EpsonProjectorComponent.Components
```

4. Drop the component into any page:

```razor
<ProjectorRemote />
```

Optional customization:

```razor
<ProjectorRemote Title="Room A Projector" Sources="new[] { ""HDMI1"", ""HDMI2"", ""LAN"" }" />
```

## Simulator Mode

Simulator mode is enabled by default for development without hardware.

Simulator behavior:

- Tracks power state transitions
- Tracks mute state and active source
- Returns realistic command responses
- Supports full UI interaction without network dependencies

## Project Structure

```
/Components
  ProjectorRemote.razor

/Extensions
  ServiceCollectionExtensions.cs

/Epson.Projector
  EpsonProjectorClient.cs
  /Models
  /Interfaces

/Epson.Projector.Transport
  IProjectorTransport.cs
  HttpProjectorTransport.cs
  SimulatedProjectorTransport.cs
  /Auth

/Pages
/Shared
/Services
```

## Epson Web API Notes

This project aligns with Epson Web API concepts including:

- REST-based control endpoints
- Digest authentication using web control credentials
- Remote control operations for power, mute, and source

API support depends on projector model capabilities and configuration.

## Running the Application

1. Clone the repository
2. Build and run the Blazor Server project
3. The application starts in simulator mode by default
4. To use a real projector, supply device IP/credentials and disable simulator mode

## Scope and Intent

The implementation prioritizes clarity and maintainability over feature breadth.  
It is intended as a practical demonstration of an AV device client/driver approach that can be extended in production systems.
