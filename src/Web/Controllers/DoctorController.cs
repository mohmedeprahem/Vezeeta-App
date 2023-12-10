using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
