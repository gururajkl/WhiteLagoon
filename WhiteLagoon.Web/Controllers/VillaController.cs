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
}
