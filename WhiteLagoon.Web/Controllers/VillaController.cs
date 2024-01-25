using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers;

public class VillaController : Controller
{
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
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(villa);
    }
}
