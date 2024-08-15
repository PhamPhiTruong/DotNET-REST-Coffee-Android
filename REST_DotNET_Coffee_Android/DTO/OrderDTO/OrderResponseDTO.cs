#nullable disable

public class OrderResponseDTO
{
    public String PaymentMethod {  get; set; }

    public string UserName { get; set; }

    public List<OrderItemResponseDTO> ItemOrderList { get; set; }

    public double TotalPrice {
        get
        {
            return ItemOrderList?.Sum(item => item.TotalPrice) ?? 0;
        }
    }
}
