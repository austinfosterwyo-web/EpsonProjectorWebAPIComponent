using EpsonProjectorComponent.Epson.Projector.Interfaces;
using EpsonProjectorComponent.Epson.Projector.Models;
using EpsonProjectorComponent.Epson.Projector.Transport;

namespace EpsonProjectorComponent.Epson.Projector;

public sealed class EpsonProjectorClient : IEpsonProjectorClient
{
    private readonly IProjectorTransport _transport;

    public EpsonProjectorClient(IProjectorTransport transport)
    {
        _transport = transport;
    }

    public Task<ProjectorCommandResult> ConnectAsync(ProjectorConnectionOptions options, CancellationToken cancellationToken = default)
        => _transport.ConfigureAsync(options, cancellationToken);

    public Task<ProjectorCommandResult> PowerOnAsync(CancellationToken cancellationToken = default)
        => _transport.PowerAsync(true, cancellationToken);

    public Task<ProjectorCommandResult> PowerOffAsync(CancellationToken cancellationToken = default)
        => _transport.PowerAsync(false, cancellationToken);

    public Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default)
        => _transport.SetMuteAsync(mute, cancellationToken);

    public Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default)
        => _transport.SetSourceAsync(source, cancellationToken);

    public Task<ProjectorStatus> GetStatusAsync(CancellationToken cancellationToken = default)
        => _transport.GetStatusAsync(cancellationToken);
}
