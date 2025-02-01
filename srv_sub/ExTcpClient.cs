using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace srv_sub
{
    public class ExTcpClient
    {
        private readonly IConfiguration _configuration;
        private readonly TcpClient _client;
        public event MessageReceivedHandler _MessageReceived;
        private NetworkStream stream;
        
        public TcpClient Client
        {
            get { return _client; }
        }

        public ExTcpClient(IConfiguration configuration)
        {
            _client = new TcpClient();
            _configuration = configuration;
                   }

        public async ValueTask<bool> Connect(string ipAddress, int port)
        {
            try
            {
                await _client.ConnectAsync(ipAddress, port);
                stream = _client.GetStream();

                var sendApiKey = await SendApiKey();

                if (sendApiKey)
                    return true;
                else
                    return false;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> SendApiKey()
        {
            var _apiKey = _configuration["apiKey"];
            var apiKeyBytes = Encoding.UTF8.GetBytes(_apiKey + "\n");

            using (SHA256 sha256Hash = SHA256.Create())
            {
                var hashBytes = sha256Hash.ComputeHash(apiKeyBytes);
                var hash = BitConverter.ToString(hashBytes).Replace(".", "").ToLower();

                var apiKeyBytesToSend = Encoding.UTF8.GetBytes(hash + "\n");
                await stream.WriteAsync(apiKeyBytesToSend, 0, apiKeyBytesToSend.Length);
            }

            var buffer = new byte[1024];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server response: {response}");

            if (response.Contains("Invalid API Key"))
            {
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            _client?.Close();
        }

        protected virtual async Task OnMessageReceived(string message)
        {
            _MessageReceived?.Invoke(this, message);
            await Task.CompletedTask;
        }
    }
}
