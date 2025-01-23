using Akka.Actor;
using Akka.IO;
using System.Net;

public class MainServerActor : ReceiveActor
{
    public static Props Props() => Akka.Actor.Props.Create(() => new MainServerActor());

    public MainServerActor()
    {
        Receive<Tcp.Connected>(connected =>
        {
            var handler = Context.ActorOf(ConnectionHandler.Props(Sender));
            Sender.Tell(new Tcp.Register(handler));
        });
    }
}
