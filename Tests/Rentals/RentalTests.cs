using MongoDB.Bson;
using NUnit.Framework;
using RealEstate.Rentals;

namespace Tests.Rentals
{
    [TestFixture]
    public class RentalTests 
    {
        [Test]
        public void ToDocument_RentalWithPrice_PriceAsDouble()
        {
            var rental = new Rental();
            rental.Price = 234;
            var document = rental.ToBsonDocument();

            Assert.AreEqual(document["Price"].BsonType, BsonType.Decimal128);
        }

        [Test]
        public void ToDocument_RentalWithId_IdIsAnObjectId()
        {
            var rental = new Rental();
            rental.Id = ObjectId.GenerateNewId().ToString();
            var document = rental.ToBsonDocument();

            Assert.AreEqual(document["_id"].BsonType, BsonType.ObjectId);
        }
    }
}
