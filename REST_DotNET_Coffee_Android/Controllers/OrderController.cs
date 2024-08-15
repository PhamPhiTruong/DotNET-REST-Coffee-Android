﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using REST_DotNET_Coffee_Android.Migrations;

[Route("order/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<OrderResponseDTO> GetOrder(int id)
    {
        var result = await _orderService.GetOrder(id);

        return result;
    }

    [HttpPut]
    public async Task<ActionResult<string>> Order([FromBody] OrderRequestDTO orderRequestDTO)
    { 
        // Tạo đơn hàng thông qua dịch vụ
        string result = await _orderService.CreateOrder(orderRequestDTO);

        // Trả kết quả
        return result;
    }
}

