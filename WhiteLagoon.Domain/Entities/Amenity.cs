using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Domain;

public class Amenity
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    [ForeignKey(nameof(Villa))]
    public int VillaId { get; set; }
    [ValidateNever]
    public Villa? Villa { get; set; }
}
