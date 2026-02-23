using System.Net;

namespace EpsonProjectorComponent.Epson.Projector.Transport.Auth;

public sealed class DigestAuthStrategy : IAuthStrategy
{
    public HttpClient CreateClient(string baseAddress, string? username, string? password, TimeSpan timeout)
    {
        var handler = new HttpClientHandler
        {
            Credentials = string.IsNullOrWhiteSpace(username)
                ? CredentialCache.DefaultNetworkCredentials
                : new NetworkCredential(username, password)
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseAddress),
            Timeout = timeout
        };

        return client;
    }
}
