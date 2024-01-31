namespace WhiteLagoon.Web.Controllers;

[Authorize(Roles = StaticDetails.RoleAdmin)]
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
        EntityViewModel<Amenity> amenityViewModel = new EntityViewModel<Amenity>
        {
            VillaList = GetListItems()
        };

        return View(amenityViewModel);
    }

    [HttpPost]
    public IActionResult Create(EntityViewModel<Amenity> amenityViewModel)
    {
        if (amenityViewModel.Entity!.VillaId == 0)
        {
            ModelState.AddModelError("VillaId", "Please select villa");
        }

        if (ModelState.IsValid)
        {
            unitOfWork.Amenity.Add(amenityViewModel.Entity);
            TempData["success"] = $"Amentiy - {amenityViewModel.Entity.Name} created successfully";
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
        EntityViewModel<Amenity> amenityViewModel = new();

        amenityViewModel.VillaList = GetListItems();
        amenityViewModel.Entity = unitOfWork.Amenity.Get(v => v.Id == Id);

        if (amenityViewModel.Entity is null)
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
    public IActionResult Update(EntityViewModel<Amenity> amenityViewModel)
    {
        if (amenityViewModel.Entity!.VillaId == 0)
        {
            ModelState.AddModelError("VillaId", "Please select villa");
        }

        if (ModelState.IsValid)
        {
            unitOfWork.Amenity.Update(amenityViewModel.Entity!);
            TempData["success"] = $"Amenity - {amenityViewModel.Entity.Name} updated successfully";
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
        EntityViewModel<Amenity> amenityViewModel = new();

        amenityViewModel.VillaList = GetListItems();
        amenityViewModel.Entity = unitOfWork.Amenity.Get(v => v.Id == Id);

        if (amenityViewModel.Entity != null)
        {
            return View(amenityViewModel);
        }
        return RedirectToAction("NotFoundPage", "Home");
    }

    [HttpPost]
    public IActionResult Delete(EntityViewModel<Amenity> amenityViewModel)
    {
        Amenity? amenityFromDbToDelete = unitOfWork.Amenity.Get(v => v.Id == amenityViewModel.Entity!.Id);

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
