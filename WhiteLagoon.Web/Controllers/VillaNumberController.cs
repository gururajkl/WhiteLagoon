using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers;

public class VillaNumberController : Controller
{
    // Injection of this type.
    private readonly IUnitOfWork unitOfWork;

    public VillaNumberController(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        // Include() will include the Villa to the VillaNumber as the Villa is Foreign Key. (Include props is case sensitive)
         //context.VillaNumbers.Include(v => v.Villa).ToList();
        IEnumerable<VillaNumber> villaNumbers = unitOfWork.VillaNumber.GetAll(null, "Villa");
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

        bool isVillaNumberUnique = unitOfWork.VillaNumber.Any(v => v.Villa_Number == villaNumberViewModel.VillaNumber!.Villa_Number);

        if (ModelState.IsValid && !isVillaNumberUnique)
        {
            unitOfWork.VillaNumber.Add(villaNumberViewModel.VillaNumber!);
            TempData["success"] = $"VillaNumber - {villaNumberViewModel.VillaNumber.Villa_Number} created successfully";
            unitOfWork.VillaNumber.Save();
            return RedirectToAction(nameof(Index));
        }

        if (isVillaNumberUnique)
        {
            TempData["error"] = $"Villa - {villaNumberViewModel.VillaNumber.Villa_Number} number already exists";
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
        villaNumberViewModel.VillaNumber = unitOfWork.VillaNumber.Get(v => v.Villa_Number == Id);

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
            unitOfWork.VillaNumber.Update(villaNumberViewModel.VillaNumber!);
            TempData["success"] = $"VillaNumber - {villaNumberViewModel.VillaNumber.Villa_Number} updated successfully";
            unitOfWork.VillaNumber.Save();
            return RedirectToAction(nameof(Index));
        }

        // Populate the VillaList before sending to UI.
        villaNumberViewModel.VillaList = GetListItems();
        TempData["error"] = "Something went wrong please try again later";
        return View(villaNumberViewModel);
    }
    #endregion

    #region DeleteAction
    [HttpGet]
    public IActionResult Delete(int Id)
    {
        VillaNumberViewModel villaNumberViewModel = new VillaNumberViewModel();

        villaNumberViewModel.VillaList = GetListItems();
        villaNumberViewModel.VillaNumber = unitOfWork.VillaNumber.Get(v => v.Villa_Number == Id);

        if (villaNumberViewModel.VillaNumber != null)
        {
            return View(villaNumberViewModel);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(VillaNumberViewModel villaNumberViewModel)
    {
        VillaNumber? villaNumberFromDbToDelete = unitOfWork.VillaNumber.Get(v => v.Villa_Number == villaNumberViewModel.VillaNumber!.Villa_Number);

        if (villaNumberFromDbToDelete is not null)
        {
            TempData["success"] = $"VillaNumber - {villaNumberFromDbToDelete.Villa_Number} Deleted successfully";
            unitOfWork.VillaNumber.Delete(villaNumberFromDbToDelete);
            unitOfWork.VillaNumber.Save();
            return RedirectToAction(nameof(Index));
        }
        TempData["error"] = $"VillaNumber cannot be Deleted";
        return View(villaNumberViewModel);
    }
    #endregion

    public IEnumerable<SelectListItem> GetListItems()
    {
        return unitOfWork.Villa.GetAll().Select(v => new SelectListItem
        {
            Text = v.Name,
            Value = v.Id.ToString()
        });
    }
}
