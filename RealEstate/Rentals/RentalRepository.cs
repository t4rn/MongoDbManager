using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RealEstate.DataAccess;
using System.IO;
using System.Threading.Tasks;

namespace RealEstate.Rentals
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DatabaseContext _context;

        public RentalRepository(DatabaseContext context)
        {
            _context = context;
        }

        public DatabaseContext DatabaseContext
        {
            get
            {
                return _context;
            }
        }

        public IMongoDatabase Database
        {
            get
            {
                return _context.Database;
            }
        }

        public void AddRental(Rental rental)
        {
            _context.Rentals.InsertOne(rental);
        }

        public Rental Get(string id)
        {
            return _context.Rentals.Find(x => x.Id == id).FirstOrDefault();
        }

        public async Task<BsonDocument> RunCommand(BsonDocument command)
        {
            return await _context.Database.RunCommandAsync<BsonDocument>(command);
        }

        public void Delete(string id)
        {
            _context.Rentals.FindOneAndDelete(x => x.Id == id);
        }

        public IMongoCollection<Rental> GetAll()
        {
            return _context.Rentals;
        }

        public ReplaceOneResult Update(Rental rental)
        {
            return _context.Rentals.ReplaceOne(x => x.Id == rental.Id, rental);
        }

        public void UpdateProperty(string id, UpdateDefinition<Rental> updateDefinition)
        {
            _context.Rentals.UpdateOne(x => x.Id == id, updateDefinition);
        }

        public GridFSDownloadStream GetStream(ObjectId objectId)
        {
            GridFSBucket gridFS = new GridFSBucket(_context.Database);

            return gridFS.OpenDownloadStream(objectId);
        }

        public void DeleteFile(ObjectId objectId)
        {
            GridFSBucket gridFS = new GridFSBucket(_context.Database);
            gridFS.Delete(objectId);
        }

        public ObjectId AddFile(string fileName, Stream inputStream, GridFSUploadOptions options)
        {
            var gridFS = new GridFSBucket(_context.Database);
            ObjectId imageId = gridFS.UploadFromStream(fileName, inputStream, options);

            return imageId;
        }
    }
}
