using Green_Lagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Green_Lagoon.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var villa=_context.Villas.ToList();
            return View();
        }
    }
}
