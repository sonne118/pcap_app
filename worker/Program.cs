using worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<WorkerProcess>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
