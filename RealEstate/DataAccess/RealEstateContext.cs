using MongoDB.Driver;
using RealEstate.Rentals;

namespace RealEstate.DataAccess
{
    public class RealEstateContext
    {
        public readonly IMongoDatabase Database;

        public IMongoCollection<Rental> Rentals { get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }

        public RealEstateContext()
        {
            var client = new MongoClient(Properties.Settings.Default.csDB);
            Database = client.GetDatabase(Properties.Settings.Default.dbName);
        }
    }
}
