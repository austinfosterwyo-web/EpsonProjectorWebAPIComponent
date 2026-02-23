namespace EpsonProjectorComponent.Epson.Projector.Models;

public sealed class ProjectorCommandResult
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public static ProjectorCommandResult Ok(string message) => new() { Success = true, Message = message };
    public static ProjectorCommandResult Fail(string message) => new() { Success = false, Message = message };
}
