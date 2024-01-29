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

    public VillaController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
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
            TempData["success"] = $"Villa {villaToDelete.Name} Deleted successfully";
            unitOfWork.Villa.Delete(villaToDelete);
            unitOfWork.Villa.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }
    #endregion
}
