package com.nlu.packages.dotnet_callapi.service;

import com.nlu.packages.dotnet_callapi.requestdto.LoginRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.TokenRespondeDTO;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

public class CoffeeService {
    private static final String BASE_URL = "http://192.168.1.30:5261/";
    private static Retrofit retrofit;
    //khởi tạo retrofit singleton
    private static Retrofit getRetrofitInstance() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(BASE_URL)
                    .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }
    // Khởi tạo Retrofit mặc định (không có token)
    public static CoffeeApi getClient() {
        return getRetrofitInstance().create(CoffeeApi.class);
    }

//    public static void main(String[] args) {
//        LoginRequestDTO lrd = new LoginRequestDTO("nqat0919@gmail.com","0919");
//        CoffeeService.getClient().login(lrd).enqueue(new Callback<TokenRespondeDTO>() {
//            @Override
//            public void onResponse(Call<TokenRespondeDTO> call, Response<TokenRespondeDTO> response) {
//                if (response.isSuccessful()) {
//                    System.out.println(response.body().getMessage());
//                }
//            }
//
//            @Override
//            public void onFailure(Call<TokenRespondeDTO> call, Throwable throwable) {
//                throw new RuntimeException(throwable);
//            }
//        });
//    }
}
