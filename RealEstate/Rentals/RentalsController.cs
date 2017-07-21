using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using RealEstate.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RealEstate.Rentals
{
    public class RentalsController : BaseController
    {
        public ActionResult Index(RentalsFilter filters)
        {
            List<Rental> rentals = FilerRentals(filters)
                .OrderBy(r => r.Price)
                .ToList();

            RentalsListVM model = new RentalsListVM()
            {
                Filters = filters,
                Rentals = rentals
            };

            return View(model);
        }

        private IMongoQueryable<Rental> FilerRentals(RentalsFilter filters)
        {
            var rentals = _context.Rentals.AsQueryable();

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

        // GET: Rentals
        public ActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Post(RentalVM model)
        {
            Rental rental = new Rental(model);

            _context.Rentals.InsertOne(rental);

            return RedirectToAction("Index");
        }

        public ActionResult AdjustPrice(string id)
        {
            Rental rental = GetRentalById(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            Rental rental = GetRentalById(id);
            rental.AdjustPrice(adjustPrice);
            ReplaceOneResult result = _context.Rentals.ReplaceOne(x => x.Id == id, rental);

            return RedirectToAction("Index");
        }

        private Rental GetRentalById(string id)
        {
            return _context.Rentals.Find(x => x.Id == id).FirstOrDefault();
        }

        public ActionResult Delete(string id)
        {
            Rental result = _context.Rentals.FindOneAndDelete(x => x.Id == id);
            return RedirectToAction("Index");
        }

        public string PriceDistribution()
        {
            var rentals = _context.Rentals;
            IEnumerable model = new QueryPriceDistribution().RunAggregation(rentals);

            return model.ToJson();
        }

        public ActionResult AttachImage(string id)
        {
            var rental = GetRentalById(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = GetRentalById(id);

            if (rental.HasImage())
            {
                DeleteImage(rental);
            }

            StoreImage(id, file, rental);

            return RedirectToAction("Index");
        }

        private void DeleteImage(Rental rental)
        {
            var gridFS = new GridFSBucket(_context.Database);
            gridFS.Delete(new ObjectId(rental.ImageId));
            SetRentalImageId(rental, null);
        }

        private void SetRentalImageId(Rental rental, string imageId)
        {
            var updateDefinition = Builders<Rental>.Update.Set(r => r.ImageId, imageId);
            _context.Rentals.UpdateOne(x => x.Id == rental.Id, updateDefinition);
        }

        private void StoreImage(string id, HttpPostedFileBase file, Rental rental)
        {
            var gridFS = new GridFSBucket(_context.Database);
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument("contentType", file.ContentType)
            };
            var imageId = gridFS.UploadFromStream(file.FileName, file.InputStream, options);
            SetRentalImageId(rental, imageId.ToString());
        }

        public ActionResult GetImage(string imageId)
        {
            try
            {
                var gridFS = new GridFSBucket(_context.Database);

                var stream = gridFS.OpenDownloadStream(new ObjectId(imageId));
                var contentType = stream.FileInfo?.Metadata["contentType"].AsString;
                return File(stream, contentType);
            }
            catch (Exception ex)
            {
                return HttpNotFound(ex.Message);
            }
        }
    }
}
