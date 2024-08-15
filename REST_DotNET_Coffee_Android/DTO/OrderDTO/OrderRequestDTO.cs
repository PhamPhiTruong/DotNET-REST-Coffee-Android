public class OrderRequestDTO
{
    public int userId {  get; set; }

    public String methodPay {  get; set; }
    public List<OrderItemRequestDTO> orderItems { get; set; }
}
