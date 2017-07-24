using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using RealEstate.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Rentals
{
    public class RentalsController : BaseController
    {
        public RentalsController(IRentalService rentalService) : base(rentalService)
        {
        }

        public async Task<ActionResult> Index(RentalsFilter filters)
        {
            List<Rental> rentals = await _rentalService.FilerRentals(filters)
                .OrderBy(r => r.Price)
                .ToListAsync();

            RentalsListVM model = new RentalsListVM()
            {
                Filters = filters,
                Rentals = rentals
            };

            return View(model);
        }

        public ActionResult Details(string id)
        {
            Rental rental = _rentalService.GetRental(id);

            return View(rental);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RentalVM model)
        {
            Rental rental = new Rental(model);

            _rentalService.AddRental(rental);

            return RedirectToAction("Index");
        }

        public ActionResult AdjustPrice(string id)
        {
            Rental rental = _rentalService.GetRental(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            Rental rental = _rentalService.GetRental(id);
            rental.AdjustPrice(adjustPrice);
            ReplaceOneResult result = _rentalService.UpdateRental(rental);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            _rentalService.DeleteRental(id);
            return RedirectToAction("Index");
        }

        public ContentResult PriceDistribution()
        {
            IMongoCollection<Rental> rentals = _rentalService.GetAllRentals();
            IEnumerable model = new QueryPriceDistribution().RunAggregation(rentals);

            return ContentJson(model.ToJson());
        }

        public ActionResult AttachImage(string id)
        {
            var rental = _rentalService.GetRental(id);
            return View(rental);
        }

        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = _rentalService.GetRental(id);

            if (rental.HasImage())
            {
                _rentalService.DeleteImage(rental);
            }

            _rentalService.StoreImage(id, file, rental);

            return RedirectToAction("Index");
        }

        public ActionResult GetImage(string imageId)
        {
            try
            {
                GridFSDownloadStream stream = _rentalService.GetStream(imageId);
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
