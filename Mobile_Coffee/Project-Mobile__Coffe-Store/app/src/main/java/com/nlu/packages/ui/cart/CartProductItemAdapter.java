package com.nlu.packages.ui.cart;

import android.app.Activity;
import android.util.SparseBooleanArray;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CheckBox;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatButton;
import androidx.recyclerview.widget.RecyclerView;

import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.dataStore.DataStore;
import com.nlu.packages.dotnet_callapi.responsedto.CartItemResponseDTO;
import com.nlu.packages.dotnet_callapi.responsedto.CartResponseDTO;
import com.squareup.picasso.Picasso;


import org.jetbrains.annotations.NotNull;

import java.util.function.BiConsumer;

import jp.wasabeef.picasso.transformations.RoundedCornersTransformation;
import lombok.var;

public class CartProductItemAdapter extends RecyclerView.Adapter<CartProductItemAdapter.MyViewHolder> {
    private Activity context;
    private DataStore dataStore;
    private BiConsumer<CartItemResponseDTO, Integer> onDeleteHandler;
    private BiConsumer<View, CartItemResponseDTO> onChangeQuantityHandler;
    private BiConsumer<Integer, Boolean> onChooseItemHandler;
    private SparseBooleanArray checkBoxStates;

    public CartProductItemAdapter(Activity context,
                                  CartResponseDTO cartResponseDTO,
                                  BiConsumer<CartItemResponseDTO, Integer> onDeleteHandler,
                                  BiConsumer<View, CartItemResponseDTO> onChangeQuantityHandler,
                                  BiConsumer<Integer, Boolean> onChooseItemHandler){
        this.context = context;
        this.dataStore = DataStore.getInstance();
        this.onDeleteHandler = onDeleteHandler;
        this.onChangeQuantityHandler = onChangeQuantityHandler;
        this.onChooseItemHandler = onChooseItemHandler;
        this.checkBoxStates = new SparseBooleanArray();
    }

    @NonNull
    @NotNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull @NotNull ViewGroup viewGroup, int i){
        View view = LayoutInflater.from(viewGroup.getContext())
                .inflate(R.layout.fragment_items_cart, viewGroup, false);
        var myViewHolder = new MyViewHolder(view);
        return myViewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull @NotNull MyViewHolder myViewHolder,int i){
        myViewHolder.renderView(this.dataStore.getCart(),i);
    }

    @Override
    public int getItemCount() {return this.dataStore.getCart().getListItem().size();}


    class MyViewHolder extends RecyclerView.ViewHolder {
        private ImageView imageView_productAvatar;
        private TextView textView_productName;
        private TextView textView_productPrice;
        private TextView textView_productQuantity;
        private ImageButton imageButton_cartItemRemoveBtn;
        private AppCompatButton appCompatButton_cartItemQuantityPlusBtn;
        private AppCompatButton appCompatButton_cartItemQuantityMinusBtn;
        private CheckBox checkBox_cartItemChoose;
        public MyViewHolder(@NonNull @NotNull View itemView) {
            super(itemView);
            textView_productName = itemView.findViewById(R.id.cart_item_productName);
            imageView_productAvatar = itemView.findViewById(R.id.cart_item_productImage);
            textView_productPrice = itemView.findViewById(R.id.cart_item_productPrice);
            textView_productQuantity = itemView.findViewById(R.id.cart_item_productQuantity);
            imageButton_cartItemRemoveBtn = itemView.findViewById(R.id.cart_item_removeBtn);
            appCompatButton_cartItemQuantityPlusBtn = itemView.findViewById(R.id.cart_item_quantityBtn_plus);
            appCompatButton_cartItemQuantityMinusBtn = itemView.findViewById(R.id.cart_item_quantityBtn_minus);
            checkBox_cartItemChoose = itemView.findViewById(R.id.cart_item_checkBox);
        }
        public void renderView(CartResponseDTO cartDTO, int position) {

            if(cartDTO.getListItem().isEmpty()){
                return;
            }
            var item = cartDTO.getListItem().get(position);
            if(position==0){
                LinearLayout parent = (LinearLayout)(imageView_productAvatar.getParent().getParent());
                ViewGroup.MarginLayoutParams margins = (ViewGroup.MarginLayoutParams) parent.getLayoutParams();
                margins.topMargin = 0;
                margins.bottomMargin = 0;
            }
            textView_productName.setText(dataStore.getList().get(item.getProductId()-1).getName());
            Picasso.get().load(dataStore.getList().get(item.getProductId()-1).getAvatarUrl())
                    .resize(100, 100)
                    .transform(new RoundedCornersTransformation(10, 0))
                    .into(imageView_productAvatar);
            textView_productPrice.setText(item.getPreTotal()+"00đ");
            textView_productQuantity.setText(item.getQuantity()+"");
            imageButton_cartItemRemoveBtn.setOnClickListener(view ->  {
                // Execute Event Handler
                onDeleteHandler.accept(item, position);
            });
            appCompatButton_cartItemQuantityPlusBtn.setOnClickListener(button -> {
                onChangeQuantityHandler.accept(button, item);
            });
            appCompatButton_cartItemQuantityMinusBtn.setOnClickListener(button -> {
                onChangeQuantityHandler.accept(button, item);
            });
            checkBox_cartItemChoose.setChecked(checkBoxStates.get(position, false));
            checkBox_cartItemChoose.setOnCheckedChangeListener((button, isChecked) -> {
                checkBoxStates.put(position, isChecked);
                onChooseItemHandler.accept(position, isChecked);
            });
        }
    }
    public void redraw(CartResponseDTO responseDTO) {
        notifyDataSetChanged();
    }
}
