package com.nlu.packages.ui.cart;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.CheckActivity;
import com.nlu.packages.R;

import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.requestdto.CartItemRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.CartRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.OrderItemRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartItemResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.nlu.packages.utils.MyUtils;
import lombok.var;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.concurrent.Executor;
import java.util.concurrent.Executors;
import java.util.function.BiConsumer;
import java.util.stream.Collectors;

public class CartActivity extends AppCompatActivity {
    ImageButton btnBack;
    private RecyclerView recycleView_listProductInCart;
    private CartProductItemAdapter cartProductItemAdapter;
    private TextView textView_totalPrice;
    private CartResponseDTO cartResponseDTO;
    private BiConsumer<CartItemResponseDTO, Integer> onDeleteHandler;
    private Runnable onLoadHandler;
    private BiConsumer<View, CartItemResponseDTO> onChangeQuantityHandler;
    private Set<Integer> chooseSet = new HashSet<>();
    private BiConsumer<Integer, Boolean> onChooseItemHandler;
    private Runnable onCheckoutClickHandler;
    private Button btnCheckout;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        DataStore dataStore = DataStore.getInstance();
        EdgeToEdge.enable(this);
        //Định nghĩa các hàm EventHandler
        onDeleteHandler = (item, pos) -> {
            CartResponseDTO cart = dataStore.getCart();
            int cartItemId = cart.getListItem().get(pos).getItemId();
            CoffeeService.getClient()
                    .deleteCart(cartItemId)
                    .enqueue(new Callback<MessageRespondDTO>() {
                        @Override
                        public void onResponse(Call<MessageRespondDTO> call, Response<MessageRespondDTO> response) {
                            Executor executor = Executors.newSingleThreadExecutor();
                            executor.execute(() -> onLoadHandler.run());
                            runOnUiThread(() -> {
                                chooseSet.remove(pos);
                                cartProductItemAdapter.redraw(cartResponseDTO);
                            });
                        }

                        @Override
                        public void onFailure(Call<MessageRespondDTO> call, Throwable throwable) {
                            throw new RuntimeException(throwable);
                        }
                    });
        };
        onLoadHandler = () -> {
            CoffeeService.getClient()
                    .getCart(1)
                    .enqueue(new Callback<CartResponseDTO>() {
                        @Override
                        public void onResponse(Call<CartResponseDTO> call, Response<CartResponseDTO> response) {
                            if(response.isSuccessful()) {
                                cartResponseDTO = response.body();
                                dataStore.setCart(cartResponseDTO);
                                if (cartResponseDTO.getListItem().size() == 0) {
                                    // draw Empty
                                    findViewById(R.id.listViewProductsInCart_EmptyState)
                                            .setVisibility(View.VISIBLE);
                                    recycleView_listProductInCart.setVisibility(View.GONE);
                                    cartProductItemAdapter.redraw(cartResponseDTO);
                                    textView_totalPrice.setText("0đ");
                                }
                                else {
                                    // draw list
                                    findViewById(R.id.listViewProductsInCart_EmptyState)
                                            .setVisibility(View.GONE);
                                    recycleView_listProductInCart.setVisibility(View.VISIBLE);
                                    cartProductItemAdapter.redraw(cartResponseDTO);
                                    textView_totalPrice.setText(cartResponseDTO.getTotal()+"00đ");
                                }
                            }
                        }

                        @Override
                        public void onFailure(Call<CartResponseDTO> call, Throwable throwable) {
                            throw new RuntimeException(throwable);
                        }
                    });
        };
        onChangeQuantityHandler = (view, itemDTO) -> {
            int quantity = itemDTO.getQuantity();
            String tag = view.getTag().toString();
            switch (tag) {
                case "btn_minus": {
                    if (quantity == 1) break;
                    quantity--;
                }
                break;
                case "btn_plus": quantity++;
                    break;
            }
            CartItemRequestDTO requestDTO = CartItemRequestDTO.builder()
                                            .itemId(itemDTO.getItemId())
                                            .quantity(quantity)
                                            .build();
            CoffeeService.getClient()
                    .updateCart(requestDTO).enqueue(new Callback<MessageRespondDTO>() {
                        @Override
                        public void onResponse(Call<MessageRespondDTO> call, Response<MessageRespondDTO> response) {
                            Executor executor = Executors.newSingleThreadExecutor();
                            executor.execute(() -> onLoadHandler.run());
                            runOnUiThread(() -> cartProductItemAdapter.redraw(cartResponseDTO));
                        }

                        @Override
                        public void onFailure(Call<MessageRespondDTO> call, Throwable throwable) {
                            throw new RuntimeException(throwable);
                        }
                    });
        };
        onChooseItemHandler = (pos, isChecked) -> {
            if(isChecked)
                chooseSet.add(pos);
            else
                chooseSet.remove(pos);
        };
        onCheckoutClickHandler = () -> {
            Intent intent = new Intent(CartActivity.this, CheckActivity.class);
            var list = getChooseList();
            intent.putExtra("chooseList", (ArrayList<OrderItemRequestDTO>) list);
            startActivity(intent);
        };


        setContentView(R.layout.activity_cart);
        btnBack = findViewById(R.id.goBackButton);
        btnBack.setOnClickListener((e) -> finish());
        btnCheckout = findViewById(R.id.cart_items_checkout);
        cartProductItemAdapter = new CartProductItemAdapter(this,
                dataStore.getCart(),
                onDeleteHandler,
                onChangeQuantityHandler,
                onChooseItemHandler);

        recycleView_listProductInCart = findViewById(R.id.listViewProductsInCart);
        textView_totalPrice = findViewById(R.id.cart_totalPrice);
        LinearLayoutManager layoutManager = new LinearLayoutManager(this);
        recycleView_listProductInCart.setLayoutManager(layoutManager);
        recycleView_listProductInCart.setAdapter(cartProductItemAdapter);
        onLoadHandler.run();
        btnCheckout.setOnClickListener((v) -> {
            if (chooseSet.size() <= 0) {
                Toast.makeText(CartActivity.this,
                        "Giỏ hàng đang trống", Toast.LENGTH_SHORT).show();
                return;
            }
            onCheckoutClickHandler.run();
        });
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    List<OrderItemRequestDTO> getChooseList() {
        List<OrderItemRequestDTO> list = new ArrayList<>();
        var arr = chooseSet.stream().collect(Collectors.toList());
        for (int i = 0; i < chooseSet.size(); i++) {
            CartItemResponseDTO item = cartResponseDTO.getListItem().get(arr.get(i));
            OrderItemRequestDTO target = OrderItemRequestDTO.builder()
                                         .poductId(item.getProductId())
                                         .quantity(item.getQuantity())
                                         .addIngredients(item.getIngredientList()).build();
            list.add(target);
        }
        return list;
    }
}