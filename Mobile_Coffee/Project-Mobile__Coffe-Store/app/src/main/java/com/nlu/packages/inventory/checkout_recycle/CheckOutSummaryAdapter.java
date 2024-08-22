package com.nlu.packages.inventory.checkout_recycle;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.ViewGroup;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.requestdto.OrderItemRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartItemResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;
import com.squareup.picasso.Picasso;

import java.util.List;

public class CheckOutSummaryAdapter extends RecyclerView.Adapter<CheckOutSummaryViewHolder> {
    DataStore dataStore;
    Activity context;
    List<OrderItemRequestDTO> lists;
    List<ProductRespondeDTO> list;
    CartResponseDTO cart;

    public CheckOutSummaryAdapter(Activity context, List<OrderItemRequestDTO> lists) {
        this.context = context;
        this.lists = lists;
        this.dataStore= DataStore.getInstance();
        list = dataStore.getList();
        cart = dataStore.getCart();
    }

    @NonNull
    @Override
    public CheckOutSummaryViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        return new CheckOutSummaryViewHolder(LayoutInflater
                .from(parent.getContext())
                .inflate(R.layout.summary_checkout_item,parent,false));
    }

    @Override
    public void onBindViewHolder(@NonNull CheckOutSummaryViewHolder holder, int position) {

        holder.nameView.setText(list.get(lists.get(position).getPoductId()-1).getName());
        holder.priceView.setText(Double.toString(cart.getListItem().get(lists.get(position).getPoductId()-1).getPreTotal()));
        holder.quantityView.setText("X "+lists.get(position).getQuantity());
        Picasso.get().load(list.get(lists.get(position).getPoductId()-1).getAvatarUrl()).into(holder.imageView);
    }

    @Override
    public int getItemCount() {
        return lists.size();
    }
}
