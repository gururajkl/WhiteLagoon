using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers;

/// <summary>
/// Villa controller used for villa CRUD.
/// </summary>
public class VillaController : Controller
{
    private readonly IVillaRepository villaRepository;

    public VillaController(IVillaRepository villaRepository)
    {
        this.villaRepository = villaRepository;
    }

    public IActionResult Index()
    {
        IEnumerable<Villa> villas = villaRepository.GetAllVillas();
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
            villaRepository.AddVilla(villa);
            TempData["success"] = $"Villa {villa.Name} created successfully";
            villaRepository.Save();
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
        Villa? villaFromDb = villaRepository.GetVilla(v => v.Id == Id);
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
            villaRepository.UpdateVilla(villa);
            TempData["success"] = $"Villa {villa.Name} updated successfully";
            villaRepository.Save();
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
        Villa? villaFromDb = villaRepository.GetVilla(v => v.Id == Id);
        if (villaFromDb != null)
        {
            return View(villaFromDb);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(Villa villa)
    {
        Villa villaToDelete = villaRepository.GetVilla(v => v.Id == villa.Id)!;

        if (villaToDelete is not null)
        {
            TempData["success"] = $"Villa {villaToDelete.Name} Deleted successfully";
            villaRepository.DeleteVilla(villaToDelete);
            villaRepository.Save();
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }
    #endregion
}
