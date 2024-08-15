using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("havingingredients")]
[PrimaryKey("ProductId", "IngredientId")]
public class HavingIngredient
{
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }

    [Required]
    [ForeignKey("Ingredient")]
    public int IngredientId { get; set; }

    public Product Product { get; set; }

    public Ingredient Ingredient { get; set; }
}