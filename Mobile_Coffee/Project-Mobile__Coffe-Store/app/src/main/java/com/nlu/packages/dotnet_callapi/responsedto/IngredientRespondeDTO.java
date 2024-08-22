package com.nlu.packages.dotnet_callapi.responsedto;

import java.io.Serializable;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class IngredientRespondeDTO implements Serializable {
    private String name;
    private double addPrice;
    private String type;
}
