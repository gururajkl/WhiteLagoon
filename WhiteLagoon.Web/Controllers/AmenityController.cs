using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Amenity> amenities = unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            AmenityViewModel amenityViewModel = new AmenityViewModel
            {
                VillaList = GetListItems()
            };

            return View(amenityViewModel);
        }

        [HttpPost]
        public IActionResult Create(AmenityViewModel amenityViewModel)
        {
            if (amenityViewModel.Amenity!.VillaId == 0)
            {
                ModelState.AddModelError("VillaId", "Please select villa");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Add(amenityViewModel.Amenity);
                TempData["success"] = $"Amentiy - {amenityViewModel.Amenity.Name} created successfully";
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            // Populate the VillaList before sending to UI.
            amenityViewModel.VillaList = GetListItems();

            return View(amenityViewModel);
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {
            AmenityViewModel amenityViewModel = new();

            amenityViewModel.VillaList = GetListItems();
            amenityViewModel.Amenity = unitOfWork.Amenity.Get(v => v.Id == Id);

            if (amenityViewModel.Amenity is null)
            {
                // Not found page.
                return RedirectToAction("NotFoundPage", "Home");
            }
            else
            {
                return View(amenityViewModel);
            }
        }

        [HttpPost]
        public IActionResult Update(AmenityViewModel amenityViewModel)
        {
            if (amenityViewModel.Amenity!.VillaId == 0)
            {
                ModelState.AddModelError("VillaId", "Please select villa");
            }

            if (ModelState.IsValid)
            {
                unitOfWork.Amenity.Update(amenityViewModel.Amenity!);
                TempData["success"] = $"Amenity - {amenityViewModel.Amenity.Name} updated successfully";
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            // Populate the VillaList before sending to UI.
            amenityViewModel.VillaList = GetListItems();
            TempData["error"] = "Something went wrong please try again later";
            return View(amenityViewModel);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            AmenityViewModel amenityViewModel = new();

            amenityViewModel.VillaList = GetListItems();
            amenityViewModel.Amenity = unitOfWork.Amenity.Get(v => v.Id == Id);

            if (amenityViewModel.Amenity != null)
            {
                return View(amenityViewModel);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public IActionResult Delete(AmenityViewModel amenityViewModel)
        {
            Amenity? amenityFromDbToDelete = unitOfWork.Amenity.Get(v => v.Id == amenityViewModel.Amenity!.Id);

            if (amenityFromDbToDelete is not null)
            {
                TempData["success"] = $"Amenity - {amenityFromDbToDelete.Name} Deleted successfully";
                unitOfWork.Amenity.Delete(amenityFromDbToDelete);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = $"Amenity cannot be Deleted";
            return View(amenityFromDbToDelete);
        }

        // Utility method.
        public IEnumerable<SelectListItem> GetListItems()
        {
            return unitOfWork.Villa.GetAll().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
        }
    }
}
