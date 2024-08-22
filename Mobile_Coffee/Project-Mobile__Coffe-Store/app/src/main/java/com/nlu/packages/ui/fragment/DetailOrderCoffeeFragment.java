package com.nlu.packages.ui.fragment;

import android.content.Intent;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.*;
import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatButton;
import androidx.fragment.app.Fragment;
import com.bumptech.glide.Glide;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.requestdto.CartRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.nlu.packages.dotnet_callapi.service.CoffeeApi;
import com.nlu.packages.ui.cart.CartActivity;

import lombok.var;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class DetailOrderCoffeeFragment extends Fragment {

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    private String mParam1;
    private String mParam2;
    private Spinner spnSize;
    private Spinner spnMilk;
    private Spinner spnSweet;
    private Spinner spnToppings;
    private AppCompatButton minusButtonQuantitty;
    private AppCompatButton plusButtonQuantity;
    private ProductRespondeDTO productTaken;
    private TextView quantityText;
    DataStore dataStore;
    private ImageView productPicture;
    private TextView productName, priceProduct, priceTotal;
    private AppCompatButton addToCartButton;
    private CartRequestDTO requestDTO;

    public static List<String> sz = new ArrayList<>();
    public static List<String> milks = new ArrayList<>();
    public static List<String> toppings = new ArrayList<>();
    public static List<String> sweeteners = new ArrayList<>();

    public DetailOrderCoffeeFragment() {
        // Required empty public constructor
    }

    public static DetailOrderCoffeeFragment newInstance(String param1, String param2) {
        DetailOrderCoffeeFragment fragment = new DetailOrderCoffeeFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_detail_order_coffee, container, false);
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
        dataStore = DataStore.getInstance();
        productPicture = view.findViewById(R.id.productPicture);
        productName = view.findViewById(R.id.productName);
        priceProduct = view.findViewById(R.id.priceProduct);
        spnSize = view.findViewById(R.id.spinner_size);
        spnMilk = view.findViewById(R.id.spinner_milk);
        spnToppings = view.findViewById(R.id.spinner_decaf);
        spnSweet = view.findViewById(R.id.spinner_sweet);
        minusButtonQuantitty = view.findViewById(R.id.minusButtonQuantitty);
        plusButtonQuantity = view.findViewById(R.id.plusButtonQuantity);
        quantityText = view.findViewById(R.id.quantityText);
        priceTotal = view.findViewById(R.id.priceTrueTotalProduct);
        addToCartButton = view.findViewById(R.id.addToCartButton);

        productTaken = (ProductRespondeDTO) getActivity().getIntent().getSerializableExtra("productOrder");

        addToCartButton.setOnClickListener(btn -> {
            CoffeeService.getClient().addCart(requestDTO)
                    .enqueue(new Callback<MessageRespondDTO>() {
                        @Override
                        public void onResponse(Call<MessageRespondDTO> call, Response<MessageRespondDTO> response) {
                            Intent intent = new Intent(getActivity(), CartActivity.class);
                            startActivity(intent);
                        }

                        @Override
                        public void onFailure(Call<MessageRespondDTO> call, Throwable throwable) {
                            throwable.printStackTrace();
                            throw new RuntimeException(throwable);
                        }
                    });
        });

        minusButtonQuantitty.setOnClickListener(v -> {
            int currentQuantity = Integer.parseInt(quantityText.getText().toString());
            if (currentQuantity > 1) {
                quantityText.setText(String.valueOf(--currentQuantity));
            }
            calculatePrice(productTaken);
        });

        plusButtonQuantity.setOnClickListener(v -> {
            int currentQuantity = Integer.parseInt(quantityText.getText().toString());
            quantityText.setText(String.valueOf(++currentQuantity));
            calculatePrice(productTaken);
        });

        updateUI();
    }

    private void updateUI() {

        Glide.with(getContext()).load(productTaken.getAvatarUrl()).into(productPicture);
        productName.setText(productTaken.getName());
        priceProduct.setText(String.valueOf(productTaken.getBasePrice()));

        sz.clear();
        milks.clear();
        toppings.clear();
        sweeteners.clear();
//        for (ProductResponseDTO.ProductSizeDTO size : product.getAvailableSizes()) {
//            sz.add(size.getSizeEnum().name());
//            sz_Map.put(size.getSizeEnum().name(), size.getSizeEnum());
//        }

//        for (ProductResponseDTO.IngredientDTO ingredient : product.getAvailableIngredients()) {
//            switch (ingredient.getIngredientType()) {
//                case MILKS:
//                    milks.add(ingredient.getIngredientName());
//                    milks_Map.put(ingredient.getIngredientName(), ingredient);
//                    break;
//                case TOPPINGS:
//                    toppings.add(ingredient.getIngredientName());
//                    toppings_Map.put(ingredient.getIngredientName(), ingredient);
//                    break;
//                case SWEETENERS:
//                    sweeteners.add(ingredient.getIngredientName());
//                    sweeteners_Map.put(ingredient.getIngredientName(), ingredient);
//                    break;
//            }
//            calculatePrice(products.get(0));
//        }

        var spinEvent = new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> adapterView, View view, int i, long l) {
                calculatePrice(productTaken);
            }

            @Override
            public void onNothingSelected(AdapterView<?> adapterView) {
                calculatePrice(productTaken);
            }
        };

        spnSize.setOnItemSelectedListener(spinEvent);
        spnMilk.setOnItemSelectedListener(spinEvent);
        spnToppings.setOnItemSelectedListener(spinEvent);
        spnSweet.setOnItemSelectedListener(spinEvent);

        setSpinnerAdapter(spnSize, sz);
        setSpinnerAdapter(spnMilk, milks);
        setSpinnerAdapter(spnToppings, toppings);
        setSpinnerAdapter(spnSweet, sweeteners);
    }

    private void setSpinnerAdapter(Spinner spinner, List<String> items) {
        ArrayAdapter<String> adapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_spinner_item, items);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinner.setAdapter(adapter);
    }

    public static List<String> getSz() {
        return sz;
    }

    public static List<String> getMilks() {
        return milks;
    }

    public static List<String> getToppings() {
        return toppings;
    }

    public static List<String> getSweeteners() {
        return sweeteners;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        sz.clear();
        milks.clear();
        toppings.clear();
        sweeteners.clear();
    }

    public void calculatePrice(ProductRespondeDTO dto) {
        var txtSize = ((TextView) spnSize.getSelectedView());
        String sizeStr = txtSize != null ? txtSize.getText().toString() : null;

        var txtMilk = ((TextView)spnMilk.getSelectedView());
        String milk = txtMilk != null ? txtMilk.getText().toString() : null;

        var txtSweet = ((TextView)spnSweet.getSelectedView());
        String sweet = txtSweet != null ? txtSweet.getText().toString() : null;

        var txtTopping = ((TextView)spnToppings.getSelectedView());
        String topping = txtTopping != null ? txtTopping.getText().toString() : null;

        int quantity = Integer.valueOf(quantityText.getText().toString());

        priceTotal.setText("00đ");
    }
}
