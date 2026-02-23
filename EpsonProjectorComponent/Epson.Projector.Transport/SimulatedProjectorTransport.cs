using EpsonProjectorComponent.Epson.Projector.Models;

namespace EpsonProjectorComponent.Epson.Projector.Transport;

public sealed class SimulatedProjectorTransport : IProjectorTransport
{
    private readonly ProjectorStatus _status = new();

    public async Task<ProjectorCommandResult> ConfigureAsync(ProjectorConnectionOptions options, CancellationToken cancellationToken = default)
    {
        await Task.Delay(120, cancellationToken);
        _status.IsConnected = true;
        _status.LastResponse = $"Simulator connected to {options.BaseAddress}";
        _status.LastUpdatedUtc = DateTimeOffset.UtcNow;
        return ProjectorCommandResult.Ok(_status.LastResponse);
    }

    public async Task<ProjectorCommandResult> PowerAsync(bool on, CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken);
        _status.IsPoweredOn = on;
        _status.LastResponse = on ? "Power state set to ON (simulated)." : "Power state set to OFF (simulated).";
        _status.LastUpdatedUtc = DateTimeOffset.UtcNow;
        return ProjectorCommandResult.Ok(_status.LastResponse);
    }

    public async Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default)
    {
        await Task.Delay(120, cancellationToken);
        _status.IsMuted = mute;
        _status.LastResponse = mute ? "Audio muted (simulated)." : "Audio unmuted (simulated).";
        _status.LastUpdatedUtc = DateTimeOffset.UtcNow;
        return ProjectorCommandResult.Ok(_status.LastResponse);
    }

    public async Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default)
    {
        await Task.Delay(150, cancellationToken);
        _status.ActiveSource = source;
        _status.LastResponse = $"Input switched to {source} (simulated).";
        _status.LastUpdatedUtc = DateTimeOffset.UtcNow;
        return ProjectorCommandResult.Ok(_status.LastResponse);
    }

    public Task<ProjectorStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        _status.LastUpdatedUtc = DateTimeOffset.UtcNow;
        return Task.FromResult(_status);
    }
}
