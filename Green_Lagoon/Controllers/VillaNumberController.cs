using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Green_Lagoon.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        } 
        public IActionResult Index()
        {
            var villaNumbers=_context.VillaNumbers.ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _context.Villas.ToList().Select(u=>new SelectListItem 
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            ViewData["Villalist"] = list;
            return View();
        }
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
           
            if(ModelState.IsValid)
            {
                _context.VillaNumbers.Add(obj);
                _context.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa Number can not be created.";
            return View();
            
        }
        [HttpGet]
        public IActionResult Update(int villaid)
        {
            Villa? obj= _context.Villas.FirstOrDefault(x => x.Id==villaid);
            if(obj==null) 
            { 
               return RedirectToAction("Error","Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
           
            if (ModelState.IsValid)
            {
                _context.Villas.Update(obj);
                _context.SaveChanges();
                TempData["success"] = "The Villa has been updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa can not be updated.";
            return View();

        }
        [HttpGet]
        public IActionResult Delete(int villaid)
        {
            Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == villaid);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objfromDb = _context.Villas.FirstOrDefault(x=>x.Id==obj.Id);
            if(objfromDb is not null)
            {
                _context.Villas.Remove(objfromDb);
                _context.SaveChanges();
                TempData["success"] = "The Villa has been deleted successfully.";
                return RedirectToAction("Index");

            }
            TempData["error"] = "The Villa can not be deleted.";

            return View();

        }

    }
}
