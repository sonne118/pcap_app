using Akka.Actor;
using Akka.IO;
using MasterNode;
using System.Net;

public class MainServerActor : ReceiveActor
{
    public static Props Props() => Akka.Actor.Props.Create(() => new MainServerActor());

    public MainServerActor()
    {
        var mongoDbShards = new MongoDbShards();

        Receive<Tcp.Connected>(connected =>
        {
            var handler = Context.ActorOf(Akka.Actor.Props.Create(() => new ConnectionHandler(Sender, mongoDbShards)));
            Sender.Tell(new Tcp.Register(handler));
        });
    }
}

