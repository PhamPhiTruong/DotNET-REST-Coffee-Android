package com.nlu.packages.dotnet_callapi.service;

import com.nlu.packages.dotnet_callapi.requestdto.CartRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.LoginRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.RegisterRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
import com.nlu.packages.dotnet_callapi.responsedto.TokenRespondeDTO;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;

public interface CoffeeApi {
    @POST("/v1/login/")
    Call<TokenRespondeDTO> login(@Body LoginRequestDTO loginRequestDTO);

    @GET("/cart/getCart")
    Call<CartResponseDTO> getCart(@Body int userId);

    @POST("/cart/addCart")
    Call<String> addCart(@Body CartRequestDTO cartRequestDTO);

    @DELETE("/cart/deleteCart")
    Call<String> deleteCart(@Body int cartItemId);

}
