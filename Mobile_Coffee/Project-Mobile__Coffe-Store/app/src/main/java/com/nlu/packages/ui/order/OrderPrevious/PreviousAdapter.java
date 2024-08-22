package com.nlu.packages.ui.order.OrderPrevious;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.responsedto.OrderItemResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.OrderResponseDTO;

import com.nlu.packages.response_dto.product.ProductResponseDTO;
import com.squareup.picasso.Picasso;
import lombok.var;

import java.util.ArrayList;

public class PreviousAdapter extends RecyclerView.Adapter<PreviousAdapter.MyHolder> {
    Context context;
    OrderResponseDTO data;

    public PreviousAdapter(Context context) {
        this.context = context;
    }

    @NonNull
    @Override
    public MyHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_previous_order, parent, false);
        return new MyHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull MyHolder holder, int position) {
        var list = data == null ?
                new ArrayList<OrderItemResponseDTO>() : data.getItemOrderList();
        holder.renderView(list.get(position));
    }

    @Override
    public int getItemCount() {
        var list = data == null ?
                new ArrayList<OrderItemResponseDTO>() : data.getItemOrderList();
        return list.size();
    }

    class MyHolder extends RecyclerView.ViewHolder {
        TextView nameProduct;
        TextView priceTitle;
        ImageView imageView1;
        Button reorderButton;
        DataStore dataStore;
        public MyHolder(@NonNull View itemView) {
            super(itemView);
            nameProduct = itemView.findViewById(R.id.orderTitle);
            imageView1 = itemView.findViewById(R.id.orderImage);
            reorderButton = itemView.findViewById(R.id.reorderButton);
            priceTitle = itemView.findViewById(R.id.priceTitle);
            dataStore = DataStore.getInstance();
        }

        public void renderView(OrderItemResponseDTO orderItemDTO) {
            if (orderItemDTO == null) return;
            nameProduct.setText(orderItemDTO.getProductName());
            Picasso.get()
                    .load(dataStore.getList().get(data.getItemOrderList().indexOf(orderItemDTO)-1).getAvatarUrl())
                    .into(imageView1);
            priceTitle.setText(orderItemDTO.getPrice()+"đ");
        }
    }

    public void renderView(OrderResponseDTO data) {
        this.data = data;
        this.notifyDataSetChanged();
    }
}
