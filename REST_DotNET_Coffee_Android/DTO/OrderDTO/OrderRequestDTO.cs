public class OrderRequestDTO
{
    int userId {  get; set; }
    List<OrderItemRequestDTO> orderItems { get; set; }
}
