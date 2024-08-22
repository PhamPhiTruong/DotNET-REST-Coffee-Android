package com.nlu.packages.dotnet_callapi.requestdto;

import com.nlu.packages.dotnet_callapi.enumtype.EProductType;

import java.io.Serializable;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class ProductRequestDTO implements Serializable {
    private int Id;
    private String Name;
    private EProductType Type;
    private double BasePrice;
    private int Quantities;
    private boolean Active;
    private int CategoryId;
    private String AvatarUrl;
}
