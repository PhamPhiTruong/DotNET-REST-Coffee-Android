
using Microsoft.AspNetCore.Mvc;

[Route("order/")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("getOrder/")]
    public async Task<OrderResponseDTO> GetOrder(int id)
    {
        var result = await _orderService.GetOrder(id);

        return result;
    }

    [HttpPut("createOrder/")]
    public async Task<MessageRespondDTO> Order([FromBody] OrderRequestDTO orderRequestDTO)
    { 
        // Trả kết quả
        return await _orderService.CreateOrder(orderRequestDTO);
    }
}

