package com.nlu.packages.dotnet_callapi.service;

import com.nlu.packages.dotnet_callapi.requestdto.LoginRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.RegisterRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.responsedto.TokenRespondeDTO;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

public interface CoffeeApi {
    @POST("/v1/login/")
    Call<TokenRespondeDTO> login(@Body LoginRequestDTO loginRequestDTO);

    @POST("/v1/register/")
    Call<MessageRespondDTO> register(@Body RegisterRequestDTO registerRequestDTO);
}
