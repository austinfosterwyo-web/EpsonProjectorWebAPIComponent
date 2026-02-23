using EpsonProjectorComponent.Epson.Projector.Models;

namespace EpsonProjectorComponent.Epson.Projector.Transport;

public interface IProjectorTransport
{
    Task<ProjectorCommandResult> ConfigureAsync(ProjectorConnectionOptions options, CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> PowerAsync(bool on, CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default);
    Task<ProjectorStatus> GetStatusAsync(CancellationToken cancellationToken = default);
}
