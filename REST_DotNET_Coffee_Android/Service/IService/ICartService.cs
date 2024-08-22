public interface ICartService
{
    // Future service here
    Task<CartResponseDTO> GetCart(int UserId);

    Task<MessageRespondDTO> AddCart(CartRequestDTO crd);

    Task<MessageRespondDTO> DeleteItemCart(int ItemCartId);

    Task<MessageRespondDTO> UpdateItem(CartItemRequestDTO cird);
}
