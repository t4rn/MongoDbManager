using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RealEstate.DataAccess;
using System.IO;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public interface IRentalRepository
    {
        void AddRental(Rental rental);

        Rental Get(string id);

        Task<BsonDocument> RunCommand(BsonDocument command);

        void Delete(string id);

        IMongoCollection<Rental> GetAll();

        ReplaceOneResult Update(Rental rental);

        void UpdateProperty(string id, UpdateDefinition<Rental> updateDefinition);

        GridFSDownloadStream GetStream(ObjectId objectId);

        void DeleteFile(ObjectId objectId);

        ObjectId AddFile(string fileName, Stream inputStream, GridFSUploadOptions options);
    }
}
