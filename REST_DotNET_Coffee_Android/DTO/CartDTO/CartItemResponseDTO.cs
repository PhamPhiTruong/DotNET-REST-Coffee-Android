#nullable disable
public class CartItemResponseDTO
{
    public int ItemId { get; set; }
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public List<String> IngredientList { get; set; }

    public double PreTotal { get; set; }
}
