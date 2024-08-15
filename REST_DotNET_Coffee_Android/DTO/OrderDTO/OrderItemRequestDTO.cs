public class OrderItemRequestDTO
{
    int productId {  get; set; }

    int quantity { get; set; }

    List<String> addIngredients { get; set; }

}
