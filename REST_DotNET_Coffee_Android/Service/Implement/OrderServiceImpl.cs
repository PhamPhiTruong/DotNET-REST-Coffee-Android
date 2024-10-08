﻿using Microsoft.EntityFrameworkCore;

#nullable disable

public class OrderServiceImpl : AService<Order>, IOrderService
{

    public OrderServiceImpl(ApplicationDbContext context, ILogger<OrderServiceImpl> logger) : base(context, logger)
    {
    }

    //
    // Summary:
    //     Create an order with the given order Data Transfer Oject (DTO). If an order with the given
    //     DTO's is null, then it is returned null. If order items attached is null, then it is returned null.
    //     Otherwise, an insert command is made to the database for the orders and order items attached, with 
    //     the given DTO and this orders, if success, is attached to the context and returned a confirmed notification.
    //
    //
    // Parameters:
    //   keyValues:
    //     The object of order DTOs for the order to be added.
    //
    // Returns:
    //     Confirmed notification.
    //
    // Remarks:
    //     See Using CreateOrder for more information and examples.
    public async Task<String> CreateOrder(OrderRequestDTO ord)
    {
        try
        {
            // Check valid request
            if (ord == null)
            {
                return null;
            }

            // Lấy userId
            int userId = ord.UserId;

            // Check valid user
            if (userId <= 0)
            {
                throw new InvalidIdException();
            }

            // Lấy paymentId
            var payment = await _context.PaymentMethods.FirstOrDefaultAsync(pm => pm.Name.Equals(ord.MethodPay));

            int paymentId = payment.Id;

            // Check valid payment id
            if (paymentId <= 0)
            {
                throw new InvalidIdException();
            }

            double totalPrice = 0;

            var order = new Order
            {
                UserId = userId,
                PaymentId = paymentId,
                TotalPrice = totalPrice
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            int orderId = order.Id;

            List<OrderItemRequestDTO> list = ord.OrderItems;

            // Check valid request
            if (list is null || list.Count <= 0)
            {
                throw new InvalidRequest();
            }

            foreach (var item in list)
            {
                int quantity = item.Quantity;

                int productId = item.ProductId;

                var orderItem = new OrderItem
                {
                    Quantities = quantity,
                    ProductId = productId,
                    OrderId = orderId
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                int orderItemId = orderItem.Id;

                List<String> listIngredients = item.AddIngredients;

                foreach (var ingre in listIngredients)
                {
                    var ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name.Equals(ingre));

                    int ingredientId = ingredient.Id;

                    var addIngredient = new AddIngredient
                    {
                        OrderItemId = orderItemId,
                        IngredientId = ingredientId
                    };

                    _context.AddIngredients.Add(addIngredient);
                }
            }

            await _context.SaveChangesAsync();

            double TrueTotal = 0;

            foreach (var item in ord.OrderItems)
            {
                double Value = 0;
                var Product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                Value += Product.BasePrice;

                foreach (var ingre in item.AddIngredients)
                {
                    var Ingredient = await _context.Ingredients.FirstOrDefaultAsync(i => i.Name == ingre);
                    Value += Ingredient.AddPrice;
                }

                Value = Value * item.Quantity;
                TrueTotal += Value;
            }

            order.TotalPrice = TrueTotal;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return "Order created successfully";
        }
        catch (Exception ex) {
            return $"Failed to create order: {ex.Message}." + ex.InnerException;
        }              
    }

    // Summary:
    //     Finds user's order. A query is made to the database for orders, if found, is
    //     attached to the context and returned. If no order is found, then null is returned.
    //
    //
    // Returns:
    //     The order found, or null.
    //
    // Remarks:
    //     See Using GetAllProduct and GetProductById for more information and examples.
    public async Task<OrderResponseDTO> GetOrder(int userId)
    {
        if (userId <= 0)
        {
            throw new InvalidIdException();
        }

        // Tìm kiếm order và user dựa trên userId
        var orders = await _context.Orders.FirstOrDefaultAsync(o => o.UserId == userId);
       
        if (orders is null)
        {
            return null;
        }

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
