using RealEstate.Rentals;
using System;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IRentalService _rentalService;

        public BaseController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        protected ActionResult Exception(Exception ex)
        {
            return View(viewName: "Error", model: ex.ToString());
        }

        protected ActionResult Error(string error)
        {
            return View(viewName: "Error", model: error);
        }

        protected ContentResult ContentJson(string conent)
        {
            return Content(conent, "application/json");
        }
    }
}