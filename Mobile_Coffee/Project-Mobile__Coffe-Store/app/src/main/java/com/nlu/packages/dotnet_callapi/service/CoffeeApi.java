package com.nlu.packages.dotnet_callapi.service;

import com.nlu.packages.dotnet_callapi.requestdto.LoginRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.RegisterRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.responsedto.TokenRespondeDTO;

//import com.nlu.packages.response_dto.product.ProductResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
//import com.nlu.packages.response_dto.order.OrderResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.OrderResponseDTO;
//import com.nlu.packages.request_dto.cart.CartItemRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.CartRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
//import com.nlu.packages.response_dto.MessageResponseDTO;
//import com.nlu.packages.response_dto.product.ProductResponseDTO;
//import com.nlu.packages.response_dto.wishlist.WishlistRequestDTO;
import retrofit2.Call;
import retrofit2.http.*;

import java.util.List;
import java.util.Map;

public interface CoffeeApi {
    @POST("/v1/login/")
    Call<TokenRespondeDTO> login(@Body LoginRequestDTO loginRequestDTO);

    @POST("/v1/register/")
    Call<MessageRespondDTO> register(@Body RegisterRequestDTO registerRequestDTO);

    @GET("v1/product")
    Call<List<ProductRespondeDTO>> getAllProduct();

    @GET("v1/product/type")
    Call<List<ProductRespondeDTO>> getProductWithType(String typePathName, String name, Long id);

    @GET("v1/product/category/{categoryPathName}")
    Call<List<ProductRespondeDTO>> getProductWithCate (String typePathName,
                                                       @Path("categoryPathName") String categoryPathName,
                                                       String name, Long id);

    @GET("api/v1/product/{productTypePath}/{categoryTypePath}")
    Call<List<ProductRespondeDTO>> getProduct(@Path("productTypePath") String productTypePath,
                                              @Path("categoryTypePath") String categoryTypePath,
                                              @QueryMap Map<String, String> options);

    @PUT("api/v2/gio-hang")
    Call<MessageRespondDTO> putItem(@Body CartRequestDTO dto);

    @GET("api/v1/product")
    Call<List<ProductRespondeDTO>>searchProduct(@Query("ten") String name);

    @GET("api/v2/don-hang")
    Call<List<OrderResponseDTO>> getOrder();

//    Call<MessageRespondDTO> addToWishList(WishlistRequestDTO wishlistRequestDTO);
}
