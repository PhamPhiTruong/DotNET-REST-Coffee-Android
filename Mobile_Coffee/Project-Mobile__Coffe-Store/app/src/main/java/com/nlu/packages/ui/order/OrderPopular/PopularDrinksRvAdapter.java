package com.nlu.packages.ui.order.OrderPopular;

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

public class PopularDrinksRvAdapter extends RecyclerView.Adapter<PopularDrinksRvAdapter.ViewHolder> {

    Context context;
    List<ProductRespondeDTO> data;
    private final PopularDrinksRvInterface popularDrinksRvInterface;
    private List<Long> productIds = new ArrayList<>();

    public PopularDrinksRvAdapter(Context context, ArrayList<ProductRespondeDTO> data, PopularDrinksRvInterface popularDrinksRvInterface) {
        this.context = context;
        this.data = data != null ? data : new ArrayList<>();
        this.popularDrinksRvInterface = popularDrinksRvInterface;
    }

    @NonNull
    @Override
    public PopularDrinksRvAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.popular_drink_rv, parent, false);
        ViewHolder viewHolder = new ViewHolder(view, popularDrinksRvInterface);
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull PopularDrinksRvAdapter.ViewHolder holder, int position) {
        holder.textView.setText(data.get(position).getName());
        Picasso.get().load(data.get(position).getAvatarUrl()).into(holder.imageView);


        if(productIds.contains(data.get(position).getId())){
            holder.toggleButton.setChecked(true);
        }
    }

    @Override
    public int getItemCount() {
        return data.size();
    }

    //khai báo textview vói image view để chứa hình ảnh với chữ
    public class ViewHolder extends RecyclerView.ViewHolder {
        ImageView imageView;
        TextView textView;
        ToggleButton toggleButton;

        //set lại nôi dung của hình ảnh với chữ
        public ViewHolder(@NonNull View itemView, PopularDrinksRvInterface popularDrinksRvInterface) {
            super(itemView);
            imageView = itemView.findViewById(R.id.popularDrinkImageRv);
            textView = itemView.findViewById(R.id.popularDrinkTitleRv);
            toggleButton = itemView.findViewById(R.id.popularDrinkFavorite);

            //xử lý sự kiện khi và 1 hình ảnh được nhấn vào sẽ chuyển qua trang chi tiết sản phẩm
            itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    if (popularDrinksRvInterface != null) {
                        int position = getAdapterPosition();
                        if (position != RecyclerView.NO_POSITION) {
                            popularDrinksRvInterface.onItemClickPopularDrinks(position);
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
