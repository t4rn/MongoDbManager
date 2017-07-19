using MongoDB.Driver;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class HomeController : Controller
    {
        private readonly RealEstateContext _context = new RealEstateContext();

        public HomeController()
        {
        }
        public ActionResult Index()
        {
            return Json(_context.Database.Settings, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}