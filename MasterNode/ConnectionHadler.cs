using Akka.Actor;
using Akka.IO;
using System.Text;

public class ConnectionHandler : ReceiveActor
{
    private readonly IActorRef _connection;

    public static Props Props(IActorRef connection) => Akka.Actor.Props.Create(() => new ConnectionHandler(connection));

    public ConnectionHandler(IActorRef connection)
    {
        _connection = connection;

        var authActor = Context.ActorOf<AuthActor>();

        Receive<Tcp.Received>(received =>
        {
            var data = received.Data.ToArray();
            var apiKey = Encoding.UTF8.GetString(data).Trim();

            authActor.Ask<bool>(new AuthActor.Authenticate(apiKey))
                          .PipeTo(Self, success: isAuthenticated => new AuthResult(isAuthenticated));
        });

        Receive<AuthResult>(authResult =>
        {
            if (!authResult.IsAuthenticated)
            {
                var response = Encoding.UTF8.GetBytes("Invalid API Key");
                _connection.Tell(Tcp.Write.Create(ByteString.FromBytes(response)));
                _connection.Tell(Tcp.Close.Instance);
                return;
            }

            var authResponse = Encoding.UTF8.GetBytes("API Key authenticated. You can now send messages.");
            _connection.Tell(Tcp.Write.Create(ByteString.FromBytes(authResponse)));

            Become(Authenticated);
        });

        Receive<Tcp.ConnectionClosed>(closed =>
        {
            Context.Stop(Self);
        });
    }

    private void Authenticated()
    {
        var dataProcessingActor = Context.ActorOf(DataProcessingActor.Props());
        var loggingActor = Context.ActorOf(LoggingActor.Props());

        Receive<Tcp.Received>(received =>
        {
            var data = received.Data.ToArray();
            var message = Encoding.UTF8.GetString(data);

            dataProcessingActor.Tell(new DataProcessingActor.ProcessData(data));
            loggingActor.Tell(new LoggingActor.LogData(data));

            var response = Encoding.UTF8.GetBytes($"Echo: {message}");
            _connection.Tell(Tcp.Write.Create(ByteString.FromBytes(response)));
        });

        Receive<Tcp.ConnectionClosed>(closed =>
        {
            Context.Stop(Self);
        });
    }

    private class AuthResult
    {
        public bool IsAuthenticated { get; }

        public AuthResult(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
        }
    }


}
