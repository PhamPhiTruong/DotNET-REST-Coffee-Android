using Microsoft.EntityFrameworkCore;
using REST_DotNET_Coffee_Android.Entities;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

[Table("cart_add_ingredients")]
[PrimaryKey(nameof(CartItemId),nameof(IngredientId))]
public class CartAddIngredients
{
    [ForeignKey("CartItem")]
    public int CartItemId { get; set; }

    public CartItem CartItem { get; set; }


    [ForeignKey("Ingredient")]
    public int IngredientId { get; set; }

    public Ingredient Ingredient { get; set; }

}
