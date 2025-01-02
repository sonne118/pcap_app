using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbReplicaSet
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // mongodb://192.168.88.181:60000/?directConnection=true&appName=mongosh+2.3.7
            var connectionString = "mongodb://<username>:<password>@<cfg1.example.net:27019>,<cfg2.example.net:27019>,<cfg3.example.net:27019>/?replicaSet=<configReplSetName>";
            var client = new MongoClient(connectionString);

            var configServer = client.GetDatabase("admin");
            var replSetInitiateCommand = new BsonDocument
            {
                { "replSetInitiate", new BsonDocument
                    {
                        { "_id", "<configReplSetName>" },
                        { "configsvr", true },
                        { "members", new BsonArray
                            {
                                new BsonDocument
                                {
                                    { "_id", 0 },
                                    { "host", "cfg1.example.net:27019" }
                                },
                                new BsonDocument
                                {
                                    { "_id", 1 },
                                    { "host", "cfg2.example.net:27019" }
                                },
                                new BsonDocument
                                {
                                    { "_id", 2 },
                                    { "host", "cfg3.example.net:27019" }
                                }
                            }
                        }
                }
                }
            };

            var result = await configServer.RunCommandAsync<BsonDocument>(replSetInitiateCommand);
            Console.WriteLine(result.ToJson());

            await AddShards(client);
        }

        static async Task AddShards(MongoClient client)
        {
            var configServer = client.GetDatabase("admin");

            var addShard1 = new BsonDocument
            {
                { "addShard", "shard1/localhost:27018" }
            };
            var addShard2 = new BsonDocument
            {
                { "addShard", "shard2/localhost:27019" }
            };
            var addShard3 = new BsonDocument
            {
                { "addShard", "shard3/localhost:27020" }
            };

            var result1 = await configServer.RunCommandAsync<BsonDocument>(addShard1);
            var result2 = await configServer.RunCommandAsync<BsonDocument>(addShard2);
            var result3 = await configServer.RunCommandAsync<BsonDocument>(addShard3);

            Console.WriteLine(result1.ToJson());
            Console.WriteLine(result2.ToJson());
            Console.WriteLine(result3.ToJson());
        }
    }
}

