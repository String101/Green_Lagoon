using Green_Lagoon.Domain.Entities;
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
            var villas=_context.Villas.ToList();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name==obj.Description)
            {
                ModelState.AddModelError("name", "Name and Description must not be the same.");
            }
            if(ModelState.IsValid)
            {
                _context.Villas.Add(obj);
                _context.SaveChanges();
                TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa can not be created.";
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
                return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa can not be deleted.";

            return View();

        }

    }
}
