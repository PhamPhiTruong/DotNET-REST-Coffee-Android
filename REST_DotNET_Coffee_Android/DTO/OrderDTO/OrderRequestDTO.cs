#nullable disable

public class OrderRequestDTO
{
    public int UserId {  get; set; }

    public String MethodPay {  get; set; }

    public List<OrderItemRequestDTO> OrderItems { get; set; }

}
