using System.Net.Http.Json;
using EpsonProjectorComponent.Epson.Projector.Models;
using EpsonProjectorComponent.Epson.Projector.Transport.Auth;

namespace EpsonProjectorComponent.Epson.Projector.Transport;

public sealed class HttpProjectorTransport : IProjectorTransport
{
    private readonly DigestAuthStrategy _authStrategy;
    private readonly ILogger<HttpProjectorTransport> _logger;
    private const int MaxRetries = 2;
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);
    private ProjectorConnectionOptions? _options;

    public HttpProjectorTransport(DigestAuthStrategy authStrategy, ILogger<HttpProjectorTransport> logger)
    {
        _authStrategy = authStrategy;
        _logger = logger;
    }

    public async Task<ProjectorCommandResult> ConfigureAsync(ProjectorConnectionOptions options, CancellationToken cancellationToken = default)
    {
        _options = options;
        var status = await GetStatusAsync(cancellationToken);
        return status.IsConnected
            ? ProjectorCommandResult.Ok($"Connected to {options.BaseAddress}")
            : ProjectorCommandResult.Fail(status.LastResponse);
    }

    public Task<ProjectorCommandResult> PowerAsync(bool on, CancellationToken cancellationToken = default)
        => PostCommandAsync("/api/control/power", new { value = on ? "on" : "off" }, $"Power {(on ? "on" : "off")}", cancellationToken);

    public Task<ProjectorCommandResult> SetMuteAsync(bool mute, CancellationToken cancellationToken = default)
        => PostCommandAsync("/api/control/audio-mute", new { value = mute ? "on" : "off" }, $"Mute {(mute ? "on" : "off")}", cancellationToken);

    public Task<ProjectorCommandResult> SetSourceAsync(string source, CancellationToken cancellationToken = default)
        => PostCommandAsync("/api/control/source", new { value = source }, $"Source {source}", cancellationToken);

    public async Task<ProjectorStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        if (_options is null)
        {
            return new ProjectorStatus { LastResponse = "Transport is not configured." };
        }

        try
        {
            using var client = _authStrategy.CreateClient(_options.BaseAddress, _options.Username, _options.Password, Timeout);
            using var response = await client.GetAsync("/api/status", cancellationToken);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<ProjectorStatusPayload>(cancellationToken: cancellationToken);
            return new ProjectorStatus
            {
                IsConnected = true,
                IsPoweredOn = string.Equals(payload?.Power, "on", StringComparison.OrdinalIgnoreCase),
                IsMuted = string.Equals(payload?.Mute, "on", StringComparison.OrdinalIgnoreCase),
                ActiveSource = payload?.Source ?? "Unknown",
                LastResponse = "Status updated.",
                LastUpdatedUtc = DateTimeOffset.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unable to retrieve projector status from {Address}", _options.BaseAddress);
            return new ProjectorStatus
            {
                IsConnected = false,
                LastResponse = $"Status request failed: {ex.Message}",
                LastUpdatedUtc = DateTimeOffset.UtcNow
            };
        }
    }

    private async Task<ProjectorCommandResult> PostCommandAsync(string path, object body, string label, CancellationToken cancellationToken)
    {
        if (_options is null)
        {
            return ProjectorCommandResult.Fail("Transport is not configured.");
        }

        for (var attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                using var client = _authStrategy.CreateClient(_options.BaseAddress, _options.Username, _options.Password, Timeout);
                using var response = await client.PostAsJsonAsync(path, body, cancellationToken);
                response.EnsureSuccessStatusCode();
                return ProjectorCommandResult.Ok($"{label} command acknowledged.");
            }
            catch (Exception ex) when (attempt < MaxRetries)
            {
                _logger.LogInformation(ex, "Retrying command {Label}. Attempt {Attempt}", label, attempt + 1);
                await Task.Delay(250 * (attempt + 1), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Command {Label} failed", label);
                return ProjectorCommandResult.Fail($"{label} failed: {ex.Message}");
            }
        }

        return ProjectorCommandResult.Fail($"{label} failed after retries.");
    }

    private sealed class ProjectorStatusPayload
    {
        public string? Power { get; set; }
        public string? Mute { get; set; }
        public string? Source { get; set; }
    }
}
