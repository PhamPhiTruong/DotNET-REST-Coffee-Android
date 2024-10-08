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
public class OrderItemResponseDTO {
    private String productName;
    private List<String> ingredientList;
    private int quantity;
    private double price;
    public double getTotalPrice() {
        return this.price * this.quantity;
    }
}
