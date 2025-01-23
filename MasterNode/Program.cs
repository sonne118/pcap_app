using Topshelf;

class Program
{
    static void Main(string[] args)
    {
        HostFactory.Run(x =>
        {
            x.Service<StreamTcpService>(s =>
            {
                s.ConstructUsing(name => new StreamTcpService());
                s.WhenStarted(tc => tc.Start(null));
                s.WhenStopped(tc => tc.Stop(null));
            });

            x.RunAsLocalSystem();

            x.SetDescription("TCP Server using Akka.NET and Topshelf");
            x.SetDisplayName("AkkaTcpServer");
            x.SetServiceName("AkkaTcpServer");
        });
    }
}
