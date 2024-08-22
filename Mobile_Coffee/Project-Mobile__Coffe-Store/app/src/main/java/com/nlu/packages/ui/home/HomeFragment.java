package com.nlu.packages.ui.home;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.provider.Settings;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup;
import android.view.inputmethod.InputMethodManager;
import android.widget.ImageButton;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.SearchView;
import androidx.constraintlayout.widget.ConstraintLayout;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.nlu.packages.dotnet_callapi.service.CoffeeApi;
import com.nlu.packages.ui.cart.CartActivity;
import com.nlu.packages.ui.fragment.DetailProductOrderActivity;
import com.nlu.packages.ui.order.OrderFragment;
import lombok.var;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.function.Consumer;

public class HomeFragment extends Fragment implements CoffeeForYouRvInterface, TopCoffeeRvInterface {
    private DataStore dataStore;
    private ConstraintLayout constraintLayoutHome;
    private SearchView searchView;
    private androidx.appcompat.widget.AppCompatButton button;
    private ImageButton avatarButton;
    private RecyclerView coffeeForYouRv, topPickRv;
    private ArrayList<ProductRespondeDTO> coffeeForYouDataSource, topPickDataSource = new ArrayList<>();
    private LinearLayoutManager linearLayoutManager;
    CoffeForYouRvAdapter coffeForYouRvAdapter;
    TopPickRvAdapter topPickRvAdapter;
    private CoffeeApi coffeeApi;
    private Consumer<ProductRespondeDTO> onClickHandler;
    private OrderFragment orderFragment = new OrderFragment();

    public HomeFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_home, container, false);
        dataStore = DataStore.getInstance();
        // Event Listener
        onClickHandler = (productDTO) -> {
            System.out.println(productDTO.getId());
            CoffeeService.getClient()
                    .getProduct(productDTO.getId())
                    .enqueue(new Callback<ProductRespondeDTO>() {
                        @Override
                        public void onResponse(Call<ProductRespondeDTO> call, Response<ProductRespondeDTO> response) {
                            if (response.isSuccessful() && response.body() != null) {
                                ProductRespondeDTO product = response.body();
                                var intent = new Intent(getContext(), DetailProductOrderActivity.class);
                                intent.putExtra("productOrder", product);
                                startActivity(intent);
                            } else {
                                try {
                                    String errorBody = response.errorBody().string();
                                    Log.e("API_ERROR", "Error code: " + response.code() + ", headers: " + response.headers());
                                } catch (IOException e) {
                                    Log.e("API_ERROR", "Error reading error body", e);
                                }
                            }
                        }

                        @Override
                        public void onFailure(Call<ProductRespondeDTO> call, Throwable throwable) {
                            throw new RuntimeException(throwable);
                        }
                    });
        };

        //add hide soft keyboard
        constraintLayoutHome = view.findViewById(R.id.constraintLayoutHomeFragment);
        searchView = view.findViewById(R.id.searchViewHome1);
        setUpUI(view);

        //xử lý sự kiện khi nhấn vào chuyển tới order menu fragment
        button = view.findViewById(R.id.order_Button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                loadFragment(orderFragment, "OrderFragment");
                BottomNavigationView navView = getActivity().findViewById(R.id.nav_view);
                navView.setSelectedItemId(R.id.navigation_order);

            }
        });



        //setting for `coffee for you` and `top pick` data source
        getListCoffee();

        //setting for coffee for you recycle view
        coffeeForYouRv = view.findViewById(R.id.homeCoffeeForYou);

        //setting the `coffee for you` adapter
        linearLayoutManager = new LinearLayoutManager(HomeFragment.this.getContext(), LinearLayoutManager.HORIZONTAL, false);
        coffeForYouRvAdapter = new CoffeForYouRvAdapter(this.getContext(), coffeeForYouDataSource, this, onClickHandler);
        coffeeForYouRv.setLayoutManager(linearLayoutManager);
        coffeeForYouRv.setAdapter(coffeForYouRvAdapter);

        //setting for top pick recycle view
        topPickRv = view.findViewById(R.id.homeTopPick);

        //setting the top pick adapter
        linearLayoutManager = new LinearLayoutManager(HomeFragment.this.getContext(), LinearLayoutManager.HORIZONTAL, false);
        topPickRvAdapter = new TopPickRvAdapter(this.getContext(), topPickDataSource, this);
        topPickRv.setLayoutManager(linearLayoutManager);
        topPickRv.setAdapter(topPickRvAdapter);

        return view;
    }


    public void hideKeyboard(View view) {
        InputMethodManager imm = (InputMethodManager) getActivity().getSystemService(Activity.INPUT_METHOD_SERVICE);
        if (imm.isActive()) {
            imm.hideSoftInputFromWindow(view.getWindowToken(), 0);
        }
    }

    public void setUpUI(View view) {
        if (!(view instanceof SearchView)) {
            view.setOnTouchListener(new View.OnTouchListener() {
                @Override
                public boolean onTouch(View view, MotionEvent motionEvent) {
                    hideKeyboard(view);
                    return false;
                }
            });
        }
        if (view instanceof ViewGroup) {
            for (int i = 0; i < ((ViewGroup) view).getChildCount(); i++) {
                View innerView = ((ViewGroup) view).getChildAt(i);
                setUpUI(innerView);
            }
        }
    }

    private void loadFragment(Fragment fragment, String tag) {
        FragmentTransaction fragmentTransaction = getActivity().getSupportFragmentManager().beginTransaction();
        Fragment existingFragment = getActivity().getSupportFragmentManager().findFragmentByTag(tag);
        if (existingFragment == null) {
            fragmentTransaction.add(R.id.container, fragment, tag);
            fragmentTransaction.addToBackStack(null);
        } else {
            fragmentTransaction.replace(R.id.container, existingFragment, tag);
        }
        fragmentTransaction.commit();
    }

    private void loadActivity(Class<? extends AppCompatActivity> activity) {
        Intent intent = new Intent(getActivity(), activity);
        startActivity(intent);
    }

    @Override
    public void onItemClickCoffeeForYou(int position) {
        Intent intent = new Intent(HomeFragment.this.getContext(), CartActivity.class);

        intent.putExtra("ProductOrder", coffeeForYouDataSource.get(position));

        startActivity(intent);
    }

    @Override
    public void onItemClickTopCoffee(int position) {
        Intent intent = new Intent(HomeFragment.this.getContext(), CartActivity.class);

        intent.putExtra("ProductOrder", coffeeForYouDataSource.get(position));

        startActivity(intent);
    }

    //get data from api
    public void getListCoffee() {
        coffeeApi = CoffeeService.getClient();
        Call<List<ProductRespondeDTO>> call = coffeeApi.getAllProduct();
        call.enqueue(new Callback<List<ProductRespondeDTO>>() {
            @Override
            public void onResponse(Call<List<ProductRespondeDTO>> call, Response<List<ProductRespondeDTO>> response) {
                if (response.isSuccessful()) {
                    //get response data for `coffee for you`
                    List<ProductRespondeDTO> responseDTOS = response.body();
                    dataStore.setList(responseDTOS);

                    coffeeForYouDataSource = (ArrayList<ProductRespondeDTO>) responseDTOS;

                    //get response data for `top pick`
                    topPickDataSource = (ArrayList<ProductRespondeDTO>) response.body();

                    //update data to adapter
                    coffeForYouRvAdapter.updateData(responseDTOS);

                    //shuffle the data  
                    Collections.shuffle(responseDTOS);

                    //re-update data to adapter
                    topPickRvAdapter.updateData(topPickDataSource);
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
}