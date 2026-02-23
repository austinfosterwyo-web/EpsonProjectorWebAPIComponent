using EpsonProjectorComponent.Epson.Projector;
using EpsonProjectorComponent.Epson.Projector.Interfaces;
using EpsonProjectorComponent.Epson.Projector.Models;
using EpsonProjectorComponent.Epson.Projector.Transport;

namespace EpsonProjectorComponent.Services;

public sealed class ProjectorControllerService
{
    private readonly SimulatedProjectorTransport _simulatedTransport;
    private readonly HttpProjectorTransport _httpTransport;
    private IEpsonProjectorClient? _client;
    private bool _usingSimulator = true;

    public ProjectorControllerService(
        SimulatedProjectorTransport simulatedTransport,
        HttpProjectorTransport httpTransport)
    {
        _simulatedTransport = simulatedTransport;
        _httpTransport = httpTransport;
    }

    public ProjectorStatus Status { get; private set; } = new();

    public async Task<ProjectorCommandResult> ConnectAsync(ProjectorConnectionOptions options, bool useSimulator, CancellationToken cancellationToken = default)
    {
        _usingSimulator = useSimulator;
        IProjectorTransport selectedTransport = useSimulator ? _simulatedTransport : _httpTransport;
        _client = new EpsonProjectorClient(selectedTransport);

        var result = await _client.ConnectAsync(options, cancellationToken);
        Status = await _client.GetStatusAsync(cancellationToken);
        Status.LastResponse = result.Message;
        return result;
    }

    public bool IsSimulator => _usingSimulator;
    public bool IsConnected => Status.IsConnected;

    public async Task<ProjectorCommandResult> PowerOnAsync(CancellationToken cancellationToken = default)
        => await RunAndRefresh(() => GetClient().PowerOnAsync(cancellationToken), cancellationToken);

    public async Task<ProjectorCommandResult> PowerOffAsync(CancellationToken cancellationToken = default)
        => await RunAndRefresh(() => GetClient().PowerOffAsync(cancellationToken), cancellationToken);

    public async Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default)
        => await RunAndRefresh(() => GetClient().SetMuteAsync(mute, cancellationToken), cancellationToken);

    public async Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default)
        => await RunAndRefresh(() => GetClient().SetSourceAsync(source, cancellationToken), cancellationToken);

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        if (_client is null)
        {
            return;
        }

        Status = await _client.GetStatusAsync(cancellationToken);
    }

    private async Task<ProjectorCommandResult> RunAndRefresh(Func<Task<ProjectorCommandResult>> command, CancellationToken cancellationToken)
    {
        var result = await command();
        Status = await GetClient().GetStatusAsync(cancellationToken);
        Status.LastResponse = result.Message;
        return result;
    }

    private IEpsonProjectorClient GetClient()
    {
        if (_client is null)
        {
            throw new InvalidOperationException("Projector is not connected.");
        }

        return _client;
    }
}
