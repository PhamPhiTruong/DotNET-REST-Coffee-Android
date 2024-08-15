
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;

public class OrderServiceImpl : AService<OrderResponseDTO>, IOrderService
{

    public OrderServiceImpl(ApplicationDbContext context, ILogger<OrderServiceImpl> logger) : base(context, logger)
    {
    }


    public async Task<OrderResponseDTO> GetOrder(int userId)
    {
        // Tìm kiếm order và user dựa trên userId
        var orders = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);
       
        var users = await _context.Users.FirstOrDefaultAsync(u => u.Id == orders.UserId);

        var paymentMethods = await _context.PaymentMethods.ToListAsync();

        var products = await _context.Products.ToListAsync();

        var addIngredients = await _context.AddIngredients.ToListAsync();

        var ingredients = await _context.Ingredients.ToListAsync();

        var orderitems = await _context.OrderItems.Where(oi => oi.OrderId == orders.Id).ToListAsync();

        String payMethod = "";

        // Xử lý dữ liệu chi tiết sản phẩm
        List<OrderItemResponseDTO> itemList = new List<OrderItemResponseDTO>();

        foreach (var orderitem in orderitems)
        {
            var product = products.FirstOrDefault(p => p.Id == orderitem.ProductId);
            payMethod = paymentMethods.FirstOrDefault(p => p.Id == orders.PaymentId).Name; 
            if (product != null)
            {
                List<string> ingredientList = new List<string>();
                double price = product.BasePrice;

                foreach (var aIngredient in addIngredients.Where(ai => ai.OrderItemId == orderitem.Id))
                {
                    var ingredient = ingredients.FirstOrDefault(i => i.Id == aIngredient.IngredientId);
                    if (ingredient != null)
                    {
                        ingredientList.Add(ingredient.Name);
                        price += ingredient.AddPrice;
                    }
                }

                itemList.Add(new OrderItemResponseDTO
                {
                    ProductName = product.Name,
                    Quantity = orderitem.Quantities,
                    Price = price,
                    IngredientList = ingredientList
                });
            }
        }

        // Tạo OrderResponseDTO
        var result = new OrderResponseDTO
        {
            PaymentMethod = payMethod,
            UserName = users.UserName,
            ItemOrderList = itemList
        };

        return result;
    }




}
