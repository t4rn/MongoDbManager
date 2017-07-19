using MongoDB.Driver;

namespace RealEstate.DataAccess
{
    public class RealEstateContext
    {
        public readonly IMongoDatabase Database;

        public RealEstateContext()
        {
            var client = new MongoClient(Properties.Settings.Default.csDB);
            Database = client.GetDatabase(Properties.Settings.Default.dbName);
        }
    }
}
