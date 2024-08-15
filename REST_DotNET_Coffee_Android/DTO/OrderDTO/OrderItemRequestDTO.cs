#nullable disable

public class OrderItemRequestDTO
{
    public int ProductId {  get; set; }

    public int Quantity { get; set; }

    public List<String> AddIngredients { get; set; }

}
