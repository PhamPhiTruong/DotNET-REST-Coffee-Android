package com.nlu.packages.ui.login;

import android.content.Intent;
import android.os.Bundle;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Toast;
import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatButton;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;
import com.nlu.packages.R;
import com.nlu.packages.dotnet_callapi.requestdto.RegisterRequestDTO;
import com.nlu.packages.dotnet_callapi.responsedto.MessageRespondDTO;
import com.nlu.packages.dotnet_callapi.service.CoffeeService;
import com.squareup.picasso.Picasso;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import java.util.Objects;

public class SignUpActivity extends AppCompatActivity {
    ImageView imageView;
    AppCompatButton button1,button2;
    Runnable onSignUpHandler;
    EditText emailText,passwordText,confirmPasswordText, usernameText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_sign_up);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);

            emailText = findViewById(R.id.signUpEmail);
            passwordText = findViewById(R.id.signUpPassword);
            confirmPasswordText = findViewById(R.id.retypePassword);
            usernameText = findViewById(R.id.signUpName);

            //banner
            imageView=findViewById(R.id.imageView2);
            Picasso.get().load("https://athome.starbucks.com/sites/default/files/2023-08/1_CAH_Article_HeartSbxCraftedCoffee_2880x1660.jpg").into(imageView);

            //go to login activity (use for sign up soon)
            button1=findViewById(R.id.SignUpButton);
            onSignUpHandler = () -> {
                String email = emailText.getText().toString();
                String password = passwordText.getText().toString();
                String confirmPassword = confirmPasswordText.getText().toString();
                String username = usernameText.getText().toString();

                if (confirmPassword.equals(password)) {
                    CoffeeService.getClient().register(RegisterRequestDTO.builder()
                                    .email(email)
                                    .password(password)
                                    .userName(username)
                                    .build())
                            .enqueue(new Callback<MessageRespondDTO>() {
                                @Override
                                public void onResponse(Call<MessageRespondDTO> call, Response<MessageRespondDTO> response) {
                                    if (response.isSuccessful()) {
                                        System.out.println(response.body());
                                        Toast.makeText(SignUpActivity.this,
                                                response.body().getMessage(),
                                                Toast.LENGTH_SHORT).show();
                                        startActivity(new Intent(SignUpActivity.this, LoginActivity.class));
                                    }
                                    else {
                                        Toast.makeText(SignUpActivity.this,
                                                response.body().getMessage(),
                                                Toast.LENGTH_SHORT).show();
                                    }
                                }

                                @Override
                                public void onFailure(Call<MessageRespondDTO> call, Throwable throwable) {
                                    Toast.makeText(SignUpActivity.this,
                                            throwable.toString(),
                                            Toast.LENGTH_SHORT).show();
                                }
                            });
                }
                else {
                    Toast.makeText(SignUpActivity.this,
                            "Retype password doesn't match",
                            Toast.LENGTH_SHORT).show();
                }

            };
            button1.setOnClickListener(s -> onSignUpHandler.run());



            //go to login activity
            button2=findViewById(R.id.backToLogin);
            button2.setOnClickListener(view ->{
                startActivity(new Intent(SignUpActivity.this, LoginActivity.class));
            });
            return insets;
        });
    }
}