using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HavingIngredient
{
    [Key]
    [Column(Order = 1)]
    public int ProductId { get; set; }

    [Key]
    [Column(Order = 2)]
    public int IngredientId { get; set; }

    public Product Product { get; set; } = new Product();

    public Ingredient Ingredient { get; set; } = new Ingredient();
}