public interface ICartService
{
    // Future service here
    Task<CartResponseDTO> GetCart(int UserId);

    Task<String> AddCart(CartRequestDTO crd);

    Task<String> DeleteItemCart(int ItemCartId);
}
