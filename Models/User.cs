using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventsApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // [BsonElement("Name")]
        public double UserId { get; set; }

        public string Designation { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

    }
}