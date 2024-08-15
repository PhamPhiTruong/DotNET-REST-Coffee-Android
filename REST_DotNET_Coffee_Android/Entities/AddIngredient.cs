using System.ComponentModel.DataAnnotations;

public class AddIngredient
{
    [Required]
    public int OrderItemId { get; set; }

    public OrderItem OrderItem { get; set; }

    [Required]
    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; }
}