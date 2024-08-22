package com.nlu.packages;

import android.content.Intent;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatButton;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.requestdto.OrderItemRequestDTO;
import com.nlu.packages.dotnet_callapi.requestdto.OrderRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.OrderResponseDTO;
import com.nlu.packages.inventory.checkout_recycle.CheckOutSummaryAdapter;
import com.nlu.packages.dotnet_callapi.responsedto.CartResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartItemResponseDTO;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class CheckActivity extends AppCompatActivity {
    AppCompatButton deliveryButton ;
    AppCompatButton takeawayButton ;
    DataStore dataStore;
    double total;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        dataStore = DataStore.getInstance();
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_check);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        List<OrderItemRequestDTO> list =  (List<OrderItemRequestDTO>) getIntent().getSerializableExtra("chooseList");
        System.out.println(list);
        RecyclerView recyclerView = findViewById(R.id.recycleOrderSummary);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        recyclerView.setAdapter(new CheckOutSummaryAdapter(this, list));
        total = 0;
        for (OrderItemRequestDTO oir : list) {
            total += dataStore.getCart().getListItem().get(list.indexOf(oir)).getPreTotal();
        }
        //<- Thay đổi được
        TextView subtotal = findViewById(R.id.subtotal);
        subtotal.setText(total+"00đ");
        TextView truetotal = findViewById(R.id.total);
        truetotal.setText(total+"00đ");

        AppCompatButton button = findViewById(R.id.proceedToPaymentButton);
        button.setOnClickListener(v -> {
            Intent intent = new Intent(CheckActivity.this, PaymentMethodActivity.class);
            intent.putExtra("total",total);
            System.out.println("This user id"+ dataStore.getUserId());
            OrderRequestDTO ord1 = OrderRequestDTO.builder().userId(dataStore.getUserId()).orderItems(list).build();
            intent.putExtra("chosenOrder", ord1);
            startActivity(intent);
        });

        ImageButton goBack = findViewById(R.id.goBackButton);
        goBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });

        deliveryButton = findViewById(R.id.deliveryButton);
        takeawayButton = findViewById(R.id.takeawayButton);
        deliveryButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                deliveryButton.setBackground(getResources().getDrawable(R.drawable.checkout_activate_button));
                takeawayButton.setBackgroundColor(getResources().getColor(R.color.Greyscale3));
                deliveryButton.setTextColor(getResources().getColor(R.color.Greyscale3));
                takeawayButton.setTextColor(getResources().getColor(R.color.Background));
            }
        });
        takeawayButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                takeawayButton.setBackground(getResources().getDrawable(R.drawable.checkout_activate_button));
                deliveryButton.setBackgroundColor(getResources().getColor(R.color.Greyscale3));
                takeawayButton.setTextColor(getResources().getColor(R.color.Greyscale3));
                deliveryButton.setTextColor(getResources().getColor(R.color.Background));
            }
        });
    }

}