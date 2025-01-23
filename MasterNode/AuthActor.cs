using Akka.Actor;
using System.Text;
using System.Security.Cryptography;
public class AuthActor : ReceiveActor
{
    readonly string[] validApiKeys = { "valid_api_key_1", "valid_api_key_2" };
    public class Authenticate
    {
        public string ApiKey { get; }

        public Authenticate(string apiKey)
        {
            ApiKey = apiKey;
        }
    }

    public AuthActor()
    {
        ReceiveAsync<Authenticate>(async auth =>
        {
            var isAuthenticated = await AuthenticateAsync(auth.ApiKey);
            Sender.Tell(isAuthenticated);
        });
    }

    private Task<bool> AuthenticateAsync(string apiKey)
    {
        List<string> validApiKeyHash = new();

        using (SHA256 sha256Hash = SHA256.Create())
        {
            foreach (var key in validApiKeys)
            {
                var apiKeyBytesToVerify = Encoding.UTF8.GetBytes(key + "\n");
                var hashBytesToVerify = sha256Hash.ComputeHash(apiKeyBytesToVerify);
                var hashToVerify = BitConverter.ToString(hashBytesToVerify).Replace(".", "").ToLower();
                validApiKeyHash.Add(hashToVerify);
            }
        }
        return Task.FromResult(validApiKeyHash.Contains(apiKey));
    }
}


