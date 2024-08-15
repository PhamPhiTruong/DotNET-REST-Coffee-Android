#nullable disable

public class OrderItemResponseDTO
{
    public String ProductName { get; set; }

    public List<String> IngredientList { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public double TotalPrice
    {
        get
        {
            return Price * Quantity;
        }
    }
}
