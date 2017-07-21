using RealEstate.Rentals;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class InfoController : BaseController
    {
        public InfoController(IRentalService rentalService) : base(rentalService)
        {
        }

        public async Task<ActionResult> BuildInfo()
        {
            string buildInfo = await _rentalService.GetBuildInfo();
            return Content(buildInfo, "application/json");
        }
    }
}