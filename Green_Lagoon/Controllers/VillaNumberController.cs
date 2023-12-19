using Green_Lagoon.Application.Common.Interface;
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
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberViewModel villaNumber = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };


            return View(villaNumber);
        }
        [HttpPost]
        public IActionResult Create(VillaNumberViewModel obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Update(VillaNumberViewModel villaNumberViewModel)
        {


            ModelState.Remove("VillaList");
            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberViewModel.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }


            villaNumberViewModel.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
            VillaNumber? objfromDb = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberViewModel.VillaNumber.Villa_Number);
            if (objfromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objfromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa Number can not be deleted.";

            return View();

        }

    }
}