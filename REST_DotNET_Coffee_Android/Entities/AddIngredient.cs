using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
[Table("add_ingredients")]
public class AddIngredient
{
    [Required]
    [Key]
    [ForeignKey("OrderItem")]
    public int OrderItemId { get; set; }

    public OrderItem OrderItem { get; set; } = new OrderItem();

    [Required]
    [ForeignKey("Ingredient")]
    
    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; } = new Ingredient();
}