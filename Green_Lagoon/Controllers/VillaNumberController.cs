using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using Green_Lagoon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var villaNumbers=_context.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberViewModel villaNumber = new()
            {
                VillaList=_context.Villas.Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value= u.Id.ToString(),
                })
            };
            
            
            return View(villaNumber);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberViewModel obj)
        {
            bool roomNumberExists = _context.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            ModelState.Remove("VillaList");
            if(ModelState.IsValid && !roomNumberExists)
            {
                _context.VillaNumbers.Add(obj.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The Villa Number has been created successfully.";
                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number already exists";
            }
           obj.VillaList= _context.Villas.Select(u => new SelectListItem
           {
               Text = u.Name,
               Value = u.Id.ToString(),
           });
            return View(obj);
            
        }
        [HttpGet]
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberViewModel villaNumberVM = new()
            {
                VillaList = _context.Villas.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(u=>u.Villa_Number==villaNumberId)
            };
           
            if(villaNumberVM.VillaNumber==null) 
            { 
               return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
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
