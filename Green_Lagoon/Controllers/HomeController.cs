using Green_Lagoon.Application.Common.Interface;
using Green_Lagoon.Infrastructure.Repositories;
using Green_Lagoon.Models;
using Green_Lagoon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Green_Lagoon.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; 

        public HomeController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new() 
            { 
                VillaList=_unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights=1,
                CheckInDate= DateOnly.FromDateTime(DateTime.Now),

            };
            return View(homeViewModel);
        }
        [HttpPost]
        public IActionResult GetVillaByDate(int nights,DateOnly checkIndate)
        {
            Thread.Sleep(3000);
            var villaList=_unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            foreach (var villa in villaList)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }
            HomeViewModel homeViewModel = new()
            {
               CheckInDate = checkIndate,
               VillaList = villaList,
               Nights = nights

            };
            return PartialView("_VillaList", homeViewModel);
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
