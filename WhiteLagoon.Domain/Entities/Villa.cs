namespace WhiteLagoon.Domain.Entities;

/// <summary>
/// Model class for the Villa with some properties.
/// </summary>
public class Villa
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please provide name")]
    public required string Name { get; set; }
    public string? Description { get; set; }
    [Display(Name = "Square Feet")]
    public int Sqft { get; set; }
    [Display(Name = "Price Per Night")]
    public double Price { get; set; }
    [Range(1, 10, ErrorMessage = "Occupancy should not be more than 10 and less than 1")]
    public int Occupancy { get; set; }
    [Display(Name = "Image Link")]
    public string? ImageUrl { get; set; }
    // Do not add to the database and do not map this prop.
    [NotMapped]
    public IFormFile? Image { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    // Navigation property.
    [ValidateNever]
    public IEnumerable<Amenity>? VillaAmenity { get; set; }
}
