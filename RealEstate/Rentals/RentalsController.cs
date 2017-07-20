using RealEstate.Controllers;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;

namespace RealEstate.Rentals
{
    public class RentalsController : BaseController
    {
        public ActionResult Index()
        {
            var rentals = _context.Rentals.AsQueryable().ToList();
            return View(rentals);
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
    }
}
