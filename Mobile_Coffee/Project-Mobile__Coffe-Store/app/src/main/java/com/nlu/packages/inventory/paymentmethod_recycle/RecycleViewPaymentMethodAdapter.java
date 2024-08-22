package com.nlu.packages.inventory.paymentmethod_recycle;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatButton;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.R;
import lombok.var;
import org.jetbrains.annotations.NotNull;

import java.util.List;
import java.util.function.Consumer;

public class RecycleViewPaymentMethodAdapter
        extends RecyclerView.Adapter<RecycleViewPaymentMethodAdapter.PaymentMethodViewHolder> {
    Activity context;
    List<String> list;
    private int chooseMethod = -1;
    private Consumer<String> onChooseHandler;
    public RecycleViewPaymentMethodAdapter(Activity context,
                                           List<String> list,
                                           Consumer<String> onChooseHandler) {
        this.context = context;
        this.list = list;
        this.onChooseHandler = onChooseHandler;
    }

    @NonNull
    @Override
    public PaymentMethodViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        var view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.payment_method_item,parent,false);
        return new PaymentMethodViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull @NotNull RecycleViewPaymentMethodAdapter.PaymentMethodViewHolder viewHolder, int i) {
        viewHolder.renderView(list.get(i), i);
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    class PaymentMethodViewHolder extends RecyclerView.ViewHolder {
        private AppCompatButton paymentMethodButton;
        public PaymentMethodViewHolder(@NonNull @NotNull View itemView) {
            super(itemView);
            this.paymentMethodButton = itemView.findViewById(R.id.methodPayOptionButton);
        }

        public void renderView(String paymentMethod, int pos) {
            this.paymentMethodButton.setText(paymentMethod);
            if (pos == chooseMethod) {
                int color1 = ContextCompat.getColor(context, R.color.Secondary);
                paymentMethodButton.setBackgroundColor(color1);
                int color2 = ContextCompat.getColor(context, R.color.Primary);
                paymentMethodButton.setTextColor(color2);
            } else {
                var color1 = ContextCompat.getDrawable(context, R.drawable.black_input_white_stroke);
                paymentMethodButton.setBackground(color1);
                int defaultTextColor = ContextCompat.getColor(context, R.color.Greyscale1);
                paymentMethodButton.setTextColor(defaultTextColor);
            }
            this.paymentMethodButton.setOnClickListener((_v) -> {
                chooseMethod = pos;
                onChooseHandler.accept(paymentMethod);
                RecycleViewPaymentMethodAdapter.this.notifyDataSetChanged();
            });
        }
    }
}
