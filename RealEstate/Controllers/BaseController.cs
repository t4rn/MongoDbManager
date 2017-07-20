using RealEstate.DataAccess;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly RealEstateContext _context = new RealEstateContext();
    }
}