using MongoDB.Driver;
using RealEstate.Rentals;

namespace RealEstate.DataAccess
{
    public class DatabaseContext
    {
        public readonly IMongoDatabase Database;

        public IMongoCollection<Rental> Rentals { get
            {
                return Database.GetCollection<Rental>("rentals");
            }
        }

        public DatabaseContext(string connectionString, string databaseName)
        {
            MongoClient client = new MongoClient(connectionString);
            Database = client.GetDatabase(databaseName);
        }
    }
}
