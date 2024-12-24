using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class MasterNodeService
{
    public static async Task MasterNodeStartAsync()
    {
        var listener = new TcpListener(IPAddress.Any, 6142);
        listener.Start();
        Console.WriteLine("Master Node is running...");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
      
    }

  
    public static async Task  Main(string[] args)
    { 
       await MasterNodeStartAsync();
    }
}

