using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.Rentals
{
    public interface IRentalService
    {
        Task<string> GetBuildInfo();

        IMongoQueryable<Rental> FilerRentals(RentalsFilter filters);

        void AddRental(Rental rental);

        Rental GetRental(string id);

        void DeleteRental(string id);

        IMongoCollection<Rental> GetAllRentals();

        ReplaceOneResult UpdateRental(Rental rental);

        void SetRentalImageId(Rental rental, string imageId);

        GridFSDownloadStream GetStream(string imageId);

        void DeleteImage(Rental rental);

        void StoreImage(string id, HttpPostedFileBase file, Rental rental);
    }
}
