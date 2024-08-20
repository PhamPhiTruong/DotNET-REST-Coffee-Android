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
public class CartResponseDTO {
    private List<CartItemResponseDTO> listItem;
    public double getTotal() {
        double result = 0;
        for(CartItemResponseDTO cir : listItem){
            result+= cir.getPreTotal();
        }
        return result;
    }
}
