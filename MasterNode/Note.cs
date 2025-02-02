using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MasterNode
{
    public class Note
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string _Id { get; set; } = Guid.NewGuid().ToString();
        public int source_port { get; set; } = 0;
        public int dest_port { get; set; } = 0;
        public string source_ip { get; set; } = string.Empty;
        public string dest_ip { get; set; } = string.Empty;
        public string source_mac { get; set; } = string.Empty;
        public string dest_mac { get; set; } = string.Empty;
        public string proto { get; set; } = string.Empty;
    }
}
