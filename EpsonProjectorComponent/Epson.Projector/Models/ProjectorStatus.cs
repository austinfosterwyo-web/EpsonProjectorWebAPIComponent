namespace EpsonProjectorComponent.Epson.Projector.Models;

public sealed class ProjectorStatus
{
    public bool IsConnected { get; set; }
    public bool IsPoweredOn { get; set; }
    public bool IsMuted { get; set; }
    public string ActiveSource { get; set; } = "HDMI1";
    public string LastResponse { get; set; } = "Not connected";
    public DateTimeOffset LastUpdatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
