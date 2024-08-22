public interface IOrderService
{
    Task<OrderResponseDTO> GetOrder(int userId);

    Task<MessageRespondDTO> CreateOrder(OrderRequestDTO ord);

}
