using Microsoft.AspNetCore.Mvc;

namespace Green_Lagoon.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
