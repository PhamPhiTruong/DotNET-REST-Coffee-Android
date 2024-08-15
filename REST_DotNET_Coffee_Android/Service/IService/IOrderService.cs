using Microsoft.AspNetCore.Mvc;

public interface IOrderService
{
    Task<OrderResponseDTO> GetOrder(int userId);
}
