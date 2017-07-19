using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RealEstate.Models
{
    public class Rental
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Description { get; set; }

        public int NumberOfRooms { get; set; }

        public List<string> Address = new List<string>();

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
