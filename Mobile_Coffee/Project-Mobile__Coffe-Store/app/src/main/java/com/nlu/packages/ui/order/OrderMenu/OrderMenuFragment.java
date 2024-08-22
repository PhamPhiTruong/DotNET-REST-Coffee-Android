package com.nlu.packages.ui.order.OrderMenu;

import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.R;
//import com.nlu.packages.response_dto.product.ProductResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
//import com.nlu.packages.service.CoffeeApi;
import com.nlu.packages.dotnet_callapi.service.CoffeeApi;
//import com.nlu.packages.service.CoffeeService;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.nlu.packages.ui.cart.CartActivity;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

public class OrderMenuFragment extends Fragment implements OrderMenuRvInterface {
    private RecyclerView.LayoutManager layoutManager;
    public OrderMenuRvAdapter orderMenuAdapter;
    RecyclerView orderMenuRv;
    CoffeeApi coffeeApi;
    private List<ProductRespondeDTO> dataSource=new ArrayList<>();

    public OrderMenuFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_order_menu, container, false);

        //order menu recyclerview
        orderMenuRv = view.findViewById(R.id.recycleViewOrderMenu);

        //fetching data from api
        getListCoffee();

        //setting grid layout
        layoutManager = new GridLayoutManager(this.getContext(), 2);
        orderMenuRv.setLayoutManager(layoutManager);

        //setting the data source
        orderMenuAdapter = new OrderMenuRvAdapter(this.getContext(), dataSource, this);
        orderMenuRv.setAdapter(orderMenuAdapter);
        orderMenuRv.setHasFixedSize(true);

        return view;
    }

    //fetch data from api
    public void getListCoffee() {
        coffeeApi = CoffeeService.getClient();
        Call<List<ProductRespondeDTO>> call = coffeeApi.getAllProduct();
        call.enqueue(new Callback<List<ProductRespondeDTO>>() {
            @Override
            public void onResponse(Call<List<ProductRespondeDTO>> call, Response<List<ProductRespondeDTO>> response) {
                if (response.isSuccessful()) {
                    //get the response
                    dataSource = response.body();
                    orderMenuAdapter.updateData(dataSource);
                    dataSource.forEach(i-> System.out.println(i.getId()+ ""));
                } else {
                    System.out.println("lỗi lấy data");
                }
            }

            @Override
            public void onFailure(Call<List<ProductRespondeDTO>> call, Throwable throwable) {
                System.out.println(throwable.getMessage());
            }
        });
    }

    @Override
    public void onClickedMenuItem(int position) {
        Intent intent = new Intent(OrderMenuFragment.this.getContext(), CartActivity.class);

        intent.putExtra("ProductOrder", (Serializable) dataSource.get(position));

        startActivity(intent);
    }
}