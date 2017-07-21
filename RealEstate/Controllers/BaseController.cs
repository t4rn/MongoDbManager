using RealEstate.DataAccess;
using System;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly RealEstateContext _context = new RealEstateContext();

        protected ActionResult Exception(Exception ex)
        {
            return View(viewName: "Error", model: ex.ToString());
        }

        protected ActionResult Error(string error)
        {
            return View(viewName: "Error", model: error);
        }
    }
}