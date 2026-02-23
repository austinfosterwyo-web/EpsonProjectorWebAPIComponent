namespace EpsonProjectorComponent.Epson.Projector.Models;

public sealed class ProjectorConnectionOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 80;
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool UseHttps { get; set; }

    public string BaseAddress => $"{(UseHttps ? "https" : "http")}://{Host}:{Port}";
}
