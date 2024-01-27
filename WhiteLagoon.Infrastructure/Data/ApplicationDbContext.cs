using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data;

/// <summary>
/// DB Context class of application, Implemented DbContext and sets some DbSet.
/// </summary>
public class ApplicationDbContext : DbContext
{
    // Pass some dbContextOptions from the Program.cs which will indirectly sent to DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // DbSets to create a table for the model class.
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed some data to the Villa entity.
        modelBuilder.Entity<Villa>().HasData(
            new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 200,
                Sqft = 550,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            },
            new Villa
            {
                Id = 2,
                Name = "Premium Pool Villa",
                Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x401",
                Occupancy = 4,
                Price = 300,
                Sqft = 550,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            },
            new Villa
            {
                Id = 3,
                Name = "Luxury Pool Villa",
                Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x402",
                Occupancy = 4,
                Price = 400,
                Sqft = 750,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            }
        );

        // Seed some data to the VillaNumber entity.
        modelBuilder.Entity<VillaNumber>().HasData(
            new VillaNumber { Villa_Number = 101, VillaId = 1 },
            new VillaNumber { Villa_Number = 102, VillaId = 1 },
            new VillaNumber { Villa_Number = 103, VillaId = 1 },
            new VillaNumber { Villa_Number = 104, VillaId = 1 },
            new VillaNumber { Villa_Number = 201, VillaId = 2 },
            new VillaNumber { Villa_Number = 202, VillaId = 2 },
            new VillaNumber { Villa_Number = 203, VillaId = 2 },
            new VillaNumber { Villa_Number = 204, VillaId = 2 },
            new VillaNumber { Villa_Number = 301, VillaId = 3 },
            new VillaNumber { Villa_Number = 302, VillaId = 3 },
            new VillaNumber { Villa_Number = 303, VillaId = 3 },
            new VillaNumber { Villa_Number = 304, VillaId = 3 }
        );
    }
}
