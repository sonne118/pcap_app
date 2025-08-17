using Akka.Actor;
using MasterNode;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static DataProcessingActor;

public class MongoDBActor : ReceiveActor
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;

    public class WriteToMongoDB
    {
        public string Message { get; }

        public WriteToMongoDB(string message)
        {
            Message = message;
        }
    }

    public MongoDBActor(string connectionString, string databaseName)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(databaseName);

        //Receive<ProcessData>(data =>
        //{
        //    var message = Encoding.UTF8.GetString(data.Data);

        //});

        ReceiveAsync<WriteToMongoDB>(async write =>
        {
            var collection = _database.GetCollection<Note>("Notes");

            try
            {
                var document = JsonSerializer.Deserialize<Note>(write.Message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });
                var message = BsonDocument.Parse(write.Message);
                Console.WriteLine($"Writed Data: {document}");
                // await collection.InsertOneAsync(document);
                Sender.Tell("Ok");
            }
            catch (Exception)
            {
                Sender.Tell("Write operation failed");
            }
        });
    }

    public static Props Props(string connectionString, string databaseName) =>
        Akka.Actor.Props.Create(() => new MongoDBActor(connectionString, databaseName));
}
