# Epson Projector Web Remote

Blazor Server remote control UI for Epson projectors, with simulator support for development.

## What it does

- Connect to a projector (IP + optional credentials)
- Power on / off
- Audio mute on / off
- Input source selection
- Live status feedback
- Simulator mode (default)

## Project layout

- `Epson.Projector/` - device client + models + interfaces
- `Epson.Projector.Transport/` - real HTTP transport, simulator transport, auth strategies
- `Services/ProjectorControllerService.cs` - UI-facing orchestration layer
- `Components/ProjectorRemote.razor` - reusable remote component
- `Extensions/ServiceCollectionExtensions.cs` - DI registration helper

## Run locally

1. `dotnet build`
2. `dotnet run`
3. Open the app and use simulator mode (enabled by default)
4. For real hardware, disable simulator and enter host/credentials

## Reuse in another Blazor Server app

1. Add a project reference to this project
2. Register services in `Program.cs`:

```csharp
builder.Services.AddEpsonProjectorRemote();
```

3. Import component namespace (host `_Imports.razor`):

```razor
@using EpsonProjectorComponent.Components
```

4. Render the remote:

```razor
<ProjectorRemote />
```

Optional:

```razor
<ProjectorRemote Title="Room A Projector" Sources="new[] { ""HDMI1"", ""HDMI2"", ""LAN"" }" />
```

## Notes

- API endpoints in `HttpProjectorTransport` are a practical baseline and may need adjustment per Epson model/API version.
- Target framework is currently `net7.0`.
