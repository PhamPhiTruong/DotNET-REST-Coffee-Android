package com.nlu.packages.dotnet_callapi.requestdto;

import java.io.Serializable;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class RegisterRequestDTO implements Serializable {
    private String userName;
    private String email;
    private String password;
    private String firstName;
    private String lastName;
    private String gender;
    private String phone;
}
