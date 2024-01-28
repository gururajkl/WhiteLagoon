using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController : Controller
{
    // Injection of this type.
    private readonly ApplicationDbContext context;

    public VillaNumberController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Index()
    {
        // Include() will include the Villa to the VillaNumber as the Villa is FK.
        List<VillaNumber> villaNumbers = context.VillaNumbers.Include(v => v.Villa).ToList();
        return View(villaNumbers);
    }

    #region CreateAction
    [HttpGet]
    public IActionResult Create()
    {
        VillaNumberViewModel villaNumber = new VillaNumberViewModel();
        villaNumber.VillaList = GetListItems();
        return View(villaNumber);
    }

    [HttpPost]
    public IActionResult Create(VillaNumberViewModel villaNumberViewModel)
    {
        if (villaNumberViewModel.VillaNumber!.VillaId == 0)
        {
            ModelState.AddModelError("VillaId", "Please select villa");
        }

        bool isVillaNumberUnique = context.VillaNumbers.Any(v => v.Villa_Number == villaNumberViewModel.VillaNumber!.Villa_Number);

        if (ModelState.IsValid && !isVillaNumberUnique)
        {
            context.VillaNumbers.Add(villaNumberViewModel.VillaNumber!);
            TempData["success"] = $"VillaNumber created successfully";
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        if (isVillaNumberUnique)
        {
            TempData["error"] = "Villa number already exists";
        }

        // Populate the VillaList before sending to UI.
        villaNumberViewModel.VillaList = GetListItems();

        return View(villaNumberViewModel);
    }
    #endregion

    #region UpdateAction
    [HttpGet]
    public IActionResult Update(int Id)
    {
        VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel();

        villaNumberViewModel.VillaList = GetListItems();
        villaNumberViewModel.VillaNumber = context.VillaNumbers.FirstOrDefault(v => v.Villa_Number == Id);

        if (villaNumberViewModel.VillaNumber is null)
        {
            // Not found page.
            return RedirectToAction("NotFoundPage", "Home");
        }
        else
        {
            return View(villaNumberViewModel);
        }
    }

    [HttpPost]
    public IActionResult Update(VillaNumberViewModel villaNumberViewModel)
    {
        if (villaNumberViewModel.VillaNumber!.VillaId == 0)
        {
            ModelState.AddModelError("VillaId", "Please select villa");
        }

        if (ModelState.IsValid)
        {
            context.VillaNumbers.Update(villaNumberViewModel.VillaNumber!);
            TempData["success"] = $"VillaNumber update successfully";
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Populate the VillaList before sending to UI.
        villaNumberViewModel.VillaList = GetListItems();

        return View(villaNumberViewModel);
    }
    #endregion

    #region DeleteAction
    [HttpGet]
    public IActionResult Delete(int Id)
    {
        VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel();

        villaNumberViewModel.VillaList = GetListItems();
        villaNumberViewModel.VillaNumber = context.VillaNumbers.FirstOrDefault(v => v.Villa_Number == Id);

        if (villaNumberViewModel.VillaNumber != null)
        {
            return View(villaNumberViewModel);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(VillaNumberViewModel villaNumberViewModel)
    {
        VillaNumber? villaNumberFromDbToDelete = context.VillaNumbers.FirstOrDefault(v => v.Villa_Number == villaNumberViewModel.VillaNumber!.Villa_Number);

        if (villaNumberFromDbToDelete is not null)
        {
            TempData["success"] = $"VillaNumber Deleted successfully";
            context.VillaNumbers.Remove(villaNumberFromDbToDelete);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        TempData["error"] = $"VillaNumber cannot be Deleted";
        return View(villaNumberViewModel);
    }
    #endregion

    public IEnumerable<SelectListItem> GetListItems()
    {
        return context.Villas.ToList().Select(v => new SelectListItem
        {
            Text = v.Name,
            Value = v.Id.ToString()
        });
    }
}
