using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers;

/// <summary>
/// Villa controller used for villa CRUD.
/// </summary>
public class VillaController : Controller
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IWebHostEnvironment webHostEnvironment;

    public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        this.unitOfWork = unitOfWork;
        this.webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        IEnumerable<Villa> villas = unitOfWork.Villa.GetAll();
        return View(villas);
    }

    #region CreateAction
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Villa villa)
    {
        // Villa name and description should not be same.
        if (villa.Name == villa.Description)
        {
            ModelState.AddModelError("Name", "Villa name and description should not be same.");
        }

        if (ModelState.IsValid)
        {
            // If the image file is uploaded.
            if (villa.Image is not null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(webHostEnvironment.WebRootPath + @"\images\VillaImages");

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(fileStream);

                villa.ImageUrl = @"images/VillaImages/" + fileName;
            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x400";
            }

            unitOfWork.Villa.Add(villa);
            TempData["success"] = $"Villa {villa.Name} created successfully";
            unitOfWork.Villa.Save();
            return RedirectToAction(nameof(Index));
        }
        TempData["error"] = $"There is some problem please review";
        return View(villa);
    }
    #endregion

    #region UpdateAction
    [HttpGet]
    public IActionResult Update(int Id)
    {
        Villa? villaFromDb = unitOfWork.Villa.Get(v => v.Id == Id);
        if (villaFromDb != null)
        {
            return View(villaFromDb);
        }

        // Not found page.
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Update(Villa villa)
    {
        // Villa name and description should not be same.
        if (villa.Name == villa.Description)
        {
            ModelState.AddModelError("Name", "Villa name and description should not be same.");
        }

        if (ModelState.IsValid)
        {
            // If the image file is uploaded.
            if (villa.Image is not null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(webHostEnvironment.WebRootPath + @"\images\VillaImages");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                villa.Image.CopyTo(fileStream);

                villa.ImageUrl = @"images/VillaImages/" + fileName;
            }

            unitOfWork.Villa.UpdateVilla(villa);
            TempData["success"] = $"Villa {villa.Name} updated successfully";
            unitOfWork.Villa.Save();
            return RedirectToAction(nameof(Index));
        }
        TempData["error"] = $"There is some problem please review";
        return View(villa);
    }
    #endregion

    #region DeleteAction
    [HttpGet]
    public IActionResult Delete(int Id)
    {
        Villa? villaFromDb = unitOfWork.Villa.Get(v => v.Id == Id);
        if (villaFromDb != null)
        {
            return View(villaFromDb);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(Villa villa)
    {
        Villa villaToDelete = unitOfWork.Villa.Get(v => v.Id == villa.Id)!;

        if (villaToDelete is not null)
        {
            // Delete the existing image.
            if (!string.IsNullOrEmpty(villaToDelete.ImageUrl))
            {
                string oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, villaToDelete.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            TempData["success"] = $"Villa {villaToDelete.Name} Deleted successfully";
            unitOfWork.Villa.Delete(villaToDelete);
            unitOfWork.Villa.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }
    #endregion
}
