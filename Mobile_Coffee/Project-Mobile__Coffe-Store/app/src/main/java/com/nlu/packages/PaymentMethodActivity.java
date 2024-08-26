package com.nlu.packages;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatButton;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.inventory.paymentmethod_recycle.RecycleViewPaymentMethodAdapter;
import com.nlu.packages.dotnet_callapi.requestdto.OrderRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.function.Consumer;

public class PaymentMethodActivity extends AppCompatActivity {
    private String method = "";
    private Consumer<String> onChoosePaymentHandler;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_payment_method);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        List<String> payMethods = new ArrayList<>();
        payMethods.add("COD");
        payMethods.add("MOMO");
        payMethods.add("DebitCard");

        // Event handler
        onChoosePaymentHandler = (payment) -> {
            method = payment;
            System.out.println(method);
        };
        RecyclerView recyclerView = findViewById(R.id.paymentMethodRecycle);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        recyclerView.setAdapter(new RecycleViewPaymentMethodAdapter(this, payMethods, onChoosePaymentHandler));
        double total = getIntent().getDoubleExtra("total",0.0);
        AppCompatButton button = findViewById(R.id.proceedButton);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                OrderRequestDTO orderReq = (OrderRequestDTO)
                        getIntent().getSerializableExtra("chosenOrder");
                if (method == null) {
                    Toast.makeText(PaymentMethodActivity.this, "Cần chọn 1 phương thức thanh toán",
                            Toast.LENGTH_SHORT).show();
                    return;
                }
                else if (method == "COD") {
                    CoffeeService.getClient().createOrder(orderReq
                                    .builder()
                                    .methodPay(method)
                                    .build()
                            ).enqueue(new Callback<MessageRespondDTO>() {
                                @Override
                                public void onResponse(Call<MessageRespondDTO> call, Response<MessageRespondDTO> response) {
                                    System.out.println(orderReq);
                                    System.out.println("Hello"+orderReq.getUserId());
                                    System.out.println(method);
                                    System.out.println(response.body().getMessage());
                                    System.out.println(response.errorBody());
                                    System.out.println(response.raw());
                                    if (response.isSuccessful()) {
                                        startActivity(new Intent(PaymentMethodActivity.this, EndPaymentActivity.class));
                                    }
                                }

                                @Override
                                public void onFailure(Call<MessageRespondDTO> call, Throwable throwable) {
                                    throw new RuntimeException(throwable);
                                }
                            });

                } else {
                    Intent intent = new Intent(PaymentMethodActivity.this,PaymentActivity.class);
                    intent.putExtra("total",total);
                    intent.putExtra("orderReq", orderReq
                            .builder()
                            .methodPay(method)
                            .build()
                    );
                    startActivity(intent);
                }
            }
        });
        ImageButton goBack = findViewById(R.id.goBackButton);
        goBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        TextView totalText = findViewById(R.id.total);
        totalText.setText(String.valueOf(total));
    }
}