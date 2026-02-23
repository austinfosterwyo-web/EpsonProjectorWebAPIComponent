namespace EpsonProjectorComponent.Epson.Projector.Transport.Auth;

public interface IAuthStrategy
{
    HttpClient CreateClient(string baseAddress, string? username, string? password, TimeSpan timeout);
}
