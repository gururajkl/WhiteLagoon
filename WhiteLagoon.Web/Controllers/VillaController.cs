using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

/// <summary>
/// Villa controller used for villa CRUD.
/// </summary>
public class VillaController : Controller
{
    // Injection of this type.
    private readonly ApplicationDbContext context;

    public VillaController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        List<Villa> villas = context.Villas.ToList();
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
            context.Villas.Add(villa);
            TempData["success"] = $"Villa {villa.Name} created successfully";
            context.SaveChanges();
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
        Villa? villaFromDb = context.Villas.FirstOrDefault(v => v.Id == Id);
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
            context.Villas.Update(villa);
            TempData["success"] = $"Villa {villa.Name} updated successfully";
            context.SaveChanges();
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
        Villa? villaFromDb = context.Villas.FirstOrDefault(v => v.Id == Id);
        if (villaFromDb != null)
        {
            return View(villaFromDb);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(Villa villa)
    {
        Villa villaToDelete = context.Villas.FirstOrDefault(v => v.Id == villa.Id)!;

        if (villaToDelete is not null)
        {
            TempData["success"] = $"Villa {villa.Name} Deleted successfully";
            context.Villas.Remove(villaToDelete);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }
    #endregion
}
