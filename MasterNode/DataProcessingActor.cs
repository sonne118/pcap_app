using Akka.Actor;
using System.Text;

public class DataProcessingActor : ReceiveActor
{
    public class ProcessData
    {
        public byte[] Data { get; }

        public ProcessData(byte[] data)
        {
            Data = data;
        }
    }

    public static Props Props() => Akka.Actor.Props.Create(() => new DataProcessingActor());

    public DataProcessingActor()
    {
        Receive<ProcessData>(data =>
        {
            var message = Encoding.UTF8.GetString(data.Data);

            Console.WriteLine($"Processed: {message}");
        });
    }
}
