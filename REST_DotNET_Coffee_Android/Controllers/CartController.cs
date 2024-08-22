using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[Route("cart/")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("getCart/")]
    public async Task<CartResponseDTO> GetCart(int UserId)
    {
        var result = await _cartService.GetCart(UserId);

        return result;
    }

    [HttpPost("addCart/")]
    public async Task<MessageRespondDTO> AddCart([FromBody] CartRequestDTO cartRequestDTO) {
        
        var result = await _cartService.AddCart(cartRequestDTO);

        return result;
    }

    [HttpDelete("deleteCart/")]
    public async Task<MessageRespondDTO> DeleteCartItem(int CartItemId)
    {
        var result = await _cartService.DeleteItemCart(CartItemId);

        return result;
    }

    [HttpPut("updateCart/")]
    public async Task<MessageRespondDTO> UpdateCartItem([FromBody] CartItemRequestDTO cartItemRequestDTO)
    {
        var result = await _cartService.UpdateItem(cartItemRequestDTO);
        return result;
    }
}
