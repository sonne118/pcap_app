using Akka.Actor;
using Akka.IO;
using MasterNode;
using System.Text;

public class ConnectionHandler : ReceiveActor
{
    private readonly IActorRef _connection;
    private readonly MongoDbShards _mongoDbShards;

    public static Props Props(IActorRef connection, MongoDbShards mongoDbShards) =>
        Akka.Actor.Props.Create(() => new ConnectionHandler(connection, mongoDbShards));

    public ConnectionHandler(IActorRef connection, MongoDbShards mongoDbShards)
    {
        _connection = connection;
        _mongoDbShards = mongoDbShards;

        var authActor = Context.ActorOf<AuthActor>();

        Receive<Tcp.Received>(received =>
        {
            var data = received.Data.ToArray();
            var apiKey = Encoding.UTF8.GetString(data).Trim();

            authActor.Ask<bool>(new AuthActor.Authenticate(apiKey))
                  .PipeTo(Self, success: isAuthenticated => new AuthResult(isAuthenticated, apiKey, data));
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

            Become(() => Authenticated(authResult.ApiKey, authResult.Data));
        });

        Receive<Tcp.ConnectionClosed>(closed =>
        {
            Context.Stop(Self);
        });
    }

    private void Authenticated(string apiKey, byte[] initialData)
    {
        var dataProcessingActor = Context.ActorOf(DataProcessingActor.Props());
        var loggingActor = Context.ActorOf(LoggingActor.Props());

        Receive<Tcp.Received>(received =>
        {
            var data = received.Data.ToArray();
            var message = Encoding.UTF8.GetString(data);

            dataProcessingActor.Tell(new DataProcessingActor.ProcessData(data));
            loggingActor.Tell(new LoggingActor.LogData(data));

            var shardKey = DetermineShard(message);
            var mongoDBActor = Context.ActorOf(MongoDBActor.Props(_mongoDbShards.GetConnectionString(shardKey), "pcap"));
            mongoDBActor.Ask<string>(new MongoDBActor.WriteToMongoDB(message))
                        .PipeTo(Self, success: writeResult => new WriteResult(writeResult));
        });

        if (initialData != null && initialData.Length > 0)
        {
            Self.Tell(new Tcp.Received(ByteString.FromBytes(initialData)));
        }

        Receive<Tcp.ConnectionClosed>(closed =>
        {
            Context.Stop(Self);
        });

        Receive<WriteResult>(writeResult =>
        {
            var response = Encoding.UTF8.GetBytes(writeResult.Result);
            _connection.Tell(Tcp.Write.Create(ByteString.FromBytes(response)));
        });
    }

    private string DetermineShard(string payload)
    {
        if (payload.Contains("\"proto\":\"TCP\""))
        {
            return "shard1";
        }
        else if (payload.Contains("\"proto\":\"UDP\""))
        {
            return "shard2";
        }
        else if (payload.Contains("\"proto\":\"TLS\""))
        {
            return "shard3";
        }
        else
        {
            return "shard4";
        }
    }

    private class AuthResult
    {
        public bool IsAuthenticated { get; }
        public string ApiKey { get; }
        public byte[] Data { get; }

        public AuthResult(bool isAuthenticated, string apiKey, byte[] data)
        {
            IsAuthenticated = isAuthenticated;
            ApiKey = apiKey;
            Data = data;
        }
    }

    private class WriteResult
    {
        public string Result { get; }

        public WriteResult(string result)
        {
            Result = result;
        }
    }
}
