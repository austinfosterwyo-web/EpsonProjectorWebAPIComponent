namespace EpsonProjectorComponent.Epson.Projector.Transport.Auth;

public sealed class NoAuthStrategy : IAuthStrategy
{
    public HttpClient CreateClient(string baseAddress, string? username, string? password, TimeSpan timeout)
    {
        return new HttpClient
        {
            BaseAddress = new Uri(baseAddress),
            Timeout = timeout
        };
    }
}
