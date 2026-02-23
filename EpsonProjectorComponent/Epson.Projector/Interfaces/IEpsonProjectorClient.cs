using EpsonProjectorComponent.Epson.Projector.Models;

namespace EpsonProjectorComponent.Epson.Projector.Interfaces;

public interface IEpsonProjectorClient
{
    Task<ProjectorCommandResult> ConnectAsync(ProjectorConnectionOptions options, CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> PowerOnAsync(CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> PowerOffAsync(CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default);
    Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default);
    Task<ProjectorStatus> GetStatusAsync(CancellationToken cancellationToken = default);
}
