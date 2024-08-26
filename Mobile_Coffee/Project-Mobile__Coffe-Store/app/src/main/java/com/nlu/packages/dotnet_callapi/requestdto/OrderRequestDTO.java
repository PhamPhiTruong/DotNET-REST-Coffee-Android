package com.nlu.packages.dotnet_callapi.requestdto;

import java.io.Serializable;
import java.util.List;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class OrderRequestDTO implements Serializable                  {
    private int userId;
    private String methodPay;
    private List<OrderItemRequestDTO> orderItems;
}
