using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System;

namespace RealEstate.Rentals
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

        public List<PriceAdjustment> Adjustments { get; set; } = new List<PriceAdjustment>();

        public string ImageId { get; internal set; }

        public Rental()
        {

        }

        public Rental(RentalVM model) : base()
        {
            Description = model.Description;
            NumberOfRooms = model.NumberOfRooms;
            Price = model.Price;
            Address = (model.Address ?? string.Empty).Split('\n').ToList();
        }


        internal void AdjustPrice(AdjustPrice adjustPrice)
        {
            var adjustment = new PriceAdjustment(adjustPrice, Price);
            Adjustments.Add(adjustment);
            Price = adjustment.NewPrice;
        }

        public bool HasImage()
        {
            return !string.IsNullOrWhiteSpace(ImageId);
        }
    }
}
