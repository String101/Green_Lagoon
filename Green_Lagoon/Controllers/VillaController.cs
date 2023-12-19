using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Domain.Entities;
using Green_Lagoon.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Green_Lagoon.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        } 
        public IActionResult Index()
        {
            var villas= _unitOfWork.Villa.GetAll();
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
                if(obj.Image != null)
                {
                    string filename= Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath,@"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, filename), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl=@"\images\VillaImage\"+filename;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x402";
                }
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa can not be created.";
            return View();
            
        }
        [HttpGet]
        public IActionResult Update(int villaid)
        {
            Villa? obj= _unitOfWork.Villa.Get(x => x.Id==villaid);
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
                if (obj.Image != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if(!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using var fileStream = new FileStream(Path.Combine(imagePath, filename), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + filename;
                }
                
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa can not be updated.";
            return View();

        }
        [HttpGet]
        public IActionResult Delete(int villaid)
        {
            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaid);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objfromDb = _unitOfWork.Villa.Get(x=>x.Id==obj.Id);
            if(objfromDb is not null)
            {
                if (!string.IsNullOrEmpty(objfromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objfromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.Villa.Remove(objfromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been deleted successfully.";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa can not be deleted.";

            return View();

        }

    }
}
