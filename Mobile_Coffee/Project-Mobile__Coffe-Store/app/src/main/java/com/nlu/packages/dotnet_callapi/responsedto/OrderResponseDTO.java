package com.nlu.packages.dotnet_callapi.responsedto;

import java.util.List;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class OrderResponseDTO {
    private String paymenMethod;
    private String userName;
    private List<OrderItemResponseDTO> itemOrderList;
    public double getTotalPrice() {
        double result =0;
        for (OrderItemResponseDTO oir : itemOrderList) {
            result += oir.getTotalPrice();
        }
        return result;
    }
}
