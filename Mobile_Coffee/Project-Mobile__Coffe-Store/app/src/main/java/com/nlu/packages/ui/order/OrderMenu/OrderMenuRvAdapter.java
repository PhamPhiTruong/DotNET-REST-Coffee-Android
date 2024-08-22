package com.nlu.packages.ui.order.OrderMenu;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ToggleButton;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.responsedto.ProductRespondeDTO;

import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.nlu.packages.dotnet_callapi.service.CoffeeApi;
import com.squareup.picasso.Picasso;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.util.ArrayList;
import java.util.List;

public class OrderMenuRvAdapter extends RecyclerView.Adapter<OrderMenuRvAdapter.ViewHolder> {

    private Context context;
    private List<ProductRespondeDTO> data;
    private final OrderMenuRvInterface orderMenuRvInterface;
    private CoffeeApi coffeeApi;
    private List<Long> productIds = new ArrayList<>();


    public OrderMenuRvAdapter(Context context, List<ProductRespondeDTO> data, OrderMenuRvInterface orderMenuRvInterface) {
        this.context = context;
        this.data = data != null ? data : new ArrayList<>();
        this.orderMenuRvInterface = orderMenuRvInterface;
    }

    @NonNull
    @Override
    public OrderMenuRvAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(R.layout.order_menu_item_rv,
                        parent, false);
        return new ViewHolder(view, orderMenuRvInterface);
    }

    @Override
    public void onBindViewHolder(@NonNull OrderMenuRvAdapter.ViewHolder holder, int position) {
        holder.productName.setText(data.get(position).getName());
        Picasso.get().load(data.get(position).getAvatarUrl()).into(holder.imageView);


        if(productIds.contains(data.get(position).getId())){
            holder.toggleButton.setChecked(true);
        }
    }

    @Override
    public int getItemCount() {
        return data.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        TextView productName;
        ImageView imageView;
        ToggleButton toggleButton;

        public ViewHolder(@NonNull View itemView, OrderMenuRvInterface orderMenuRvInterface) {
            super(itemView);

            productName = itemView.findViewById(R.id.orderMenuProductName);
            imageView = itemView.findViewById(R.id.orderMenuImageRv);
            toggleButton = itemView.findViewById(R.id.orderMenuFavorite);

            //xử lý sự kiện khi và 1 hình ảnh được nhấn vào sẽ chuyển qua trang chi tiết sản phẩm
            itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    if (orderMenuRvInterface != null) {
                        int position = getAdapterPosition();
                        if (position != RecyclerView.NO_POSITION) {
                            orderMenuRvInterface.onClickedMenuItem(position);
                        }
                    }
                }
            });
        }
    }

    public void updateData(List<ProductRespondeDTO> newList) {
        this.data.clear();
        this.data.addAll(newList);
        notifyDataSetChanged();
    }
}
