using Microsoft.EntityFrameworkCore;
using REST_DotNET_Coffee_Android.Entities;

#nullable disable
public class CartServiceImpl : AService<Cart>, ICartService
{
    public CartServiceImpl(ApplicationDbContext context, ILogger<CartServiceImpl> logger) : base(context, logger)
    {
    }

    public async Task<string> AddCart(CartRequestDTO crd)
    {
        try
        {
            int userId = crd.UserId;

            // Check valid user id
            if (userId <= 0)
            {
                throw new InvalidIdException();
            }

            var IngredientList = crd.IngredientList;

            var Cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (Cart == null)
            {
                // Tạo cart nếu chưa có
                var NewCart = new Cart
                {
                    UserId = userId
                };

                _context.Carts.Add(NewCart);
                await _context.SaveChangesAsync();

                // Thêm cart item liên quan đến cart mới tạo
                var CartItem = new CartItem
                {
                    Price = crd.PreTotal,
                    Quantity = crd.Quantity,
                    CartId = NewCart.Id,
                    ProductId = crd.ProductId
                };

                _context.CartItems.Add(CartItem);
                await _context.SaveChangesAsync();

                // Bổ sung chi tiết các thành phần ingredient cho cartItem mới vừa thêm
                foreach (var item in IngredientList)
                {
                    var Ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == item);
                    int idIngre = Ingredient.Id;
                    var CartAddIngre = new CartAddIngredients
                    {
                        CartItemId = CartItem.Id,
                        IngredientId = idIngre
                    };
                    _context.CartAddIngredients.Add(CartAddIngre);
                }
                await _context.SaveChangesAsync();
                return "Cart add successfully";
            }
            else
            {
                // Thêm cart item liên quan đến cart đã có sẵn
                var CartItem = new CartItem
                {
                    Price = crd.PreTotal,
                    Quantity = crd.Quantity,
                    CartId = Cart.Id,
                    ProductId = crd.ProductId
                };

                _context.CartItems.Add(CartItem);
                await _context.SaveChangesAsync();

                // Bổ sung chi tiết các thành phần igredient cho cartItem mới vừa thêm
                foreach (var item in IngredientList)
                {
                    var Ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == item);
                    int idIngre = Ingredient.Id;
                    var CartAddIngre = new CartAddIngredients
                    {
                        CartItemId = CartItem.Id,
                        IngredientId = idIngre
                    };
                    _context.CartAddIngredients.Add(CartAddIngre);
                }
                await _context.SaveChangesAsync();
                return "Cart add successfully";
            }
        }
        catch (Exception ex)
        {
            return $"Failed to add cart: {ex.Message}";
        }   
    }

    public async Task<string> DeleteItemCart(int ItemCartId)
    {
        try
        {
            // Lấy Cart Item liên quan
            var CartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.Id == ItemCartId);
            // Tiến hành xóa Các thành phần phụ liên quan dến cart item
            var CartIngredientList = await _context.CartAddIngredients.Where(cai => cai.CartItemId == CartItem.Id).ToListAsync();
            foreach ( var item in CartIngredientList)
            {
                _context.CartAddIngredients.Remove(item);
            }
            await _context.SaveChangesAsync();
            _context.CartItems.Remove(CartItem);
            // Xóa cart item
            await _context.SaveChangesAsync();
            return "Delete cart item successfully";
        }
        catch (Exception ex) {
            return $"Failed to delete item cart: {ex.Message}";
        }
    }

    public async Task<CartResponseDTO> GetCart(int UserId)
    {
        var Cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == UserId);

        int CartId = Cart.Id;

        List<CartItem> ListCartItem = await _context.CartItems.Where(ci => ci.CartId == CartId).ToListAsync();

        List<CartItemResponseDTO> List = new List<CartItemResponseDTO>();

        foreach (var item in ListCartItem)
        {
            int CartItemId = item.Id;
            int ProductId = item.ProductId;
            int Quantity = item.Quantity;
            List<String> Ingredients = new List<String>();
            List<CartAddIngredients> cais = await _context.CartAddIngredients.Where(cai => cai.CartItemId == item.Id).ToListAsync();
            foreach (var cai in cais)
            {
                var ingre = await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == cai.IngredientId);
                String ingreName = ingre.Name;
                Ingredients.Add(ingreName);
            }
            double PreTotal = item.Price*item.Quantity;
            List.Add(new CartItemResponseDTO
            {
                ItemId = CartItemId,
                ProductId = ProductId,
                Quantity = Quantity,
                IngredientList = Ingredients,
                PreTotal = PreTotal
            });           
        }

        CartResponseDTO result = new CartResponseDTO
        {
            listItem = List
        };
        return result;
    }


}
