package com.nlu.packages.dotnet_callapi.responsedto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class ProductRespondeDTO {
    private int id;
    private String name;
    private String type;
    private double basePrice;
    private int quantities;
    private boolean active;
    private int categoryId;
    private String avatarUrl;
}
