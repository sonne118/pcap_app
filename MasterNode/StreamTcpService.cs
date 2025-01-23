using Akka.Actor;
using Akka.IO;
using System.Net;
using Topshelf;

public class StreamTcpService : ServiceControl
{
    private ActorSystem _sys;

    public bool Start(HostControl hostControl)
    {
        _sys = ActorSystem.Create("tcp-processing");
        var server = _sys.ActorOf(MainServerActor.Props(), nameof(MainServerActor));
        var tcpManager = _sys.Tcp();

        tcpManager.Tell(new Tcp.Bind(server, new IPEndPoint(IPAddress.Any, 8000)));

        return true;
    }

    public bool Stop(HostControl hostControl)
    {
        _sys.Terminate().Wait();
        return true;
    }
}
