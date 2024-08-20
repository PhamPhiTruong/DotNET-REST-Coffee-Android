package com.nlu.packages.dotnet_callapi.responsedto;

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
public class CartItemResponseDTO implements Serializable {
    private int itemId;
    private int productId;
    private int quantity;
    private List<String> ingredientList;
    private double preTotal;
}
