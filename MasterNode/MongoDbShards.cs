using MongoDB.Bson;
using MongoDB.Driver;

namespace MasterNode
{
    public class MongoDbShards
    {
        private readonly Dictionary<string, string> _shardConnectionStrings = new()
    {
        { "shard1", "mongodb://localhost:27018" },
        { "shard2", "mongodb://localhost:27019" },
        { "shard3", "mongodb://localhost:27020" },
        { "shard4", "mongodb://localhost:27021" }
    };

        public string GetConnectionString(string shardKey)
        {
            return _shardConnectionStrings.TryGetValue(shardKey, out var connectionString)
                ? connectionString
                : _shardConnectionStrings["defaultShard"];
        }
    }

}
