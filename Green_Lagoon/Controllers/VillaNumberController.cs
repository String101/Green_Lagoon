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
                return RedirectToAction(nameof(Index));
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
        public IActionResult Update(VillaNumberViewModel villaNumberViewModel)
        {

            
            ModelState.Remove("VillaList");
            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Update(villaNumberViewModel.VillaNumber);
                _context.SaveChanges();
                TempData["success"] = "The Villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
                
            
            villaNumberViewModel.VillaList = _context.Villas.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            return View(villaNumberViewModel);

        }
        [HttpGet]
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberViewModel villaNumberVM = new()
            {
                VillaList = _context.Villas.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberViewModel villaNumberViewModel)
        {
            VillaNumber? objfromDb = _context.VillaNumbers.FirstOrDefault(x=>x.Villa_Number==villaNumberViewModel.VillaNumber.Villa_Number);
            if(objfromDb is not null)
            {
                _context.VillaNumbers.Remove(objfromDb);
                _context.SaveChanges();
                TempData["success"] = "The Villa Number has been deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa Number can not be deleted.";

            return View();

        }

    }
}
