namespace WhiteLagoon.Domain.Entities;

/// <summary>
/// Assosiate this <seealso cref="VillaNumber" /> with <seealso cref="Entities.Villa" />.
/// </summary>
public class VillaNumber
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None), Required(ErrorMessage = "Villa Number is requried")]
    public int Villa_Number { get; set; }
    [Display(Name = "Villa Name")]
    public int VillaId { get; set; }
    [ForeignKey(nameof(VillaId))]
    [ValidateNever]
    public Villa? Villa { get; set; }
    public string? SpecialDetails { get; set; }
}
