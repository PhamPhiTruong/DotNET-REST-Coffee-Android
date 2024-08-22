package com.nlu.packages.dotnet_callapi.service;

import com.nlu.packages.dotnet_callapi.requestdto.CartItemRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.CartRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.LoginRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.OrderRequestDTO;
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
import retrofit2.http.PUT;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface CoffeeApi {
    @POST("/v1/login")
    Call<TokenRespondeDTO> login(@Body LoginRequestDTO loginRequestDTO);

    @GET("/cart/getCart")
    Call<CartResponseDTO> getCart(@Query("userId") int userId);

    @POST("/cart/addCart")
    Call<MessageRespondDTO> addCart(@Body CartRequestDTO cartRequestDTO);

    @DELETE("/cart/deleteCart")
    Call<MessageRespondDTO> deleteCart(@Query("cartItemId") int cartItemId);

    @PUT("/cart/updateCart")
    Call<MessageRespondDTO> updateCart(@Body CartItemRequestDTO cartItemRequestDTO);

    @GET("/product/getAllProduct/")
    Call<List<ProductRespondeDTO>> getAllProduct();

    @GET("/product/getProductById")
    Call<ProductRespondeDTO> getProduct(@Query("id") int id);

    @PUT("order/createOrder")
    Call<MessageRespondDTO> createOrder(@Body OrderRequestDTO ord);
}
