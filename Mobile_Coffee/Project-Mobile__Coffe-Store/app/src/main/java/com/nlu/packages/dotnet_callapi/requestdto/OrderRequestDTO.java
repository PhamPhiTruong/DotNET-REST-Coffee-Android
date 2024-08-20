package com.nlu.packages.dotnet_callapi.requestdto;

import java.util.List;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class OrderRequestDTO {
    private int userId;
    private String methodPay;
    private List<OrderItemRequestDTO> orderItems;
}
