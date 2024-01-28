using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository;

public class VillaRepository : IVillaRepository
{
    // Injection of this type.
    private readonly ApplicationDbContext context;

    public VillaRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public void AddVilla(Villa villa)
    {
        context.Add(villa);
    }

    public void UpdateVilla(Villa villa)
    {
        context.Update(villa);
    }

    public void DeleteVilla(Villa villa)
    {
        context.Villas.Remove(villa);
    }

    public IEnumerable<Villa> GetAllVillas(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<Villa> villas = context.Villas.AsQueryable();

        if (filter is not null)
        {
            villas = villas.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                villas.Include(includeProperty);
            }
        }

        return villas.ToList();
    }

    public Villa GetVilla(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
    {
        IQueryable<Villa> villa = context.Villas.AsQueryable();
        villa = villa.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                villa.Include(includeProperty);
            }
        }

        return villa.FirstOrDefault()!;
    }

    public void Save()
    {
        context.SaveChanges();
    }
}
