using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventsApi.Models
{
    public class Volunteer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // [BsonElement("Name")]
        public string EmployeeId { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

    }
}