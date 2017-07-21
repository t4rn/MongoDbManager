using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.Rentals
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repo;

        public RentalService(IRentalRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> GetBuildInfo()
        {
            BsonDocument buildInfoCommand = new BsonDocument("buildinfo", 1);
            BsonDocument buildInfo = await _repo.RunCommand(buildInfoCommand);

            return buildInfo.ToJson();
        }

        public IMongoQueryable<Rental> FilerRentals(RentalsFilter filters)
        {
            var rentals = _repo.DatabaseContext.Rentals.AsQueryable();

            if (filters.PriceLimit.HasValue)
            {
                rentals = rentals.Where(x => x.Price <= filters.PriceLimit);
            }

            if (filters.MinimumRooms.HasValue)
            {
                rentals = rentals.Where(x => x.NumberOfRooms >= filters.MinimumRooms);
            }

            return rentals;
        }

        public void AddRental(Rental rental)
        {
            _repo.AddRental(rental);
        }

        public Rental GetRental(string id)
        {
            return _repo.Get(id);
        }

        public void DeleteRental(string id)
        {
            _repo.Delete(id);
        }

        public IMongoCollection<Rental> GetAllRentals()
        {
            return _repo.GetAll();
        }

        public ReplaceOneResult UpdateRental(Rental rental)
        {
            return _repo.Update(rental);
        }

        public void SetRentalImageId(Rental rental, string imageId)
        {
            UpdateDefinition<Rental> updateDefinition = Builders<Rental>.Update.Set(r => r.ImageId, imageId);
            _repo.UpdateProperty(rental.Id, updateDefinition);
        }

        public GridFSDownloadStream GetStream(string imageId)
        {
            return _repo.GetStream(new ObjectId(imageId));
        }

        public void DeleteImage(Rental rental)
        {
            // delete image
            _repo.DeleteFile(new ObjectId(rental.ImageId));

            // update rental withh null imageId
            SetRentalImageId(rental, null);
        }

        public void StoreImage(string id, HttpPostedFileBase file, Rental rental)
        {
            GridFSUploadOptions options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument("contentType", file.ContentType)
            };

            // add image
            var imageId = _repo.AddFile(file.FileName, file.InputStream, options);

            // update rental with imageId
            SetRentalImageId(rental, imageId.ToString());
        }
    }
}
