using Akka.Actor;

public class LoggingActor : ReceiveActor
{
    public class LogData
    {
        public byte[] Data { get; }

        public LogData(byte[] data)
        {
            Data = data;
        }
    }

    public static Props Props() => Akka.Actor.Props.Create(() => new LoggingActor());

    public LoggingActor()
    {
        Receive<LogData>(log =>
        {
            var message = System.Text.Encoding.UTF8.GetString(log.Data);

            Console.WriteLine($"Logged Data: {message}");
        });
    }
}
