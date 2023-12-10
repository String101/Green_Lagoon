using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Application.Common.Utility;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using Green_Lagoon.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Green_Lagoon.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class VillaAmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaAmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaAmenitiesViewModel Amenity = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };


            return View(Amenity);
        }
        [HttpPost]
        public IActionResult Create(VillaAmenitiesViewModel obj)
        {
           

            if (ModelState.IsValid )
            {
                
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Amenity.Save();
                TempData["success"] = "The Villa Amenity has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
           
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            return View(obj);

        }
        [HttpGet]
        public IActionResult Update(int Id)
        {
            VillaAmenitiesViewModel amenity = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == Id )
            };

            if (amenity.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenity);
        }
        [HttpPost]
        public IActionResult Update(VillaAmenitiesViewModel villaAmenityViewModel)
        {


            ModelState.Remove("VillaList");
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(villaAmenityViewModel.Amenity);
                _unitOfWork.Amenity.Save();
                TempData["success"] = "The Villa Amenity has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }


            villaAmenityViewModel.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            return View(villaAmenityViewModel);

        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            VillaAmenitiesViewModel amenities = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == Id)
            };

            if (amenities.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenities);
        }
        [HttpPost]
        public IActionResult Delete(VillaAmenitiesViewModel villaamenitiesViewModel)
        {
            Amenity? objfromDb = _unitOfWork.Amenity.Get(x => x.Id == villaamenitiesViewModel.Amenity.Id);
            if (objfromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objfromDb);
                _unitOfWork.Amenity.Save();
                TempData["success"] = "The Villa Amenity has been deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa Amenity can not be deleted.";

            return View();

        }

    }
}