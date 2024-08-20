using Microsoft.AspNetCore.Mvc;

public interface IUserService : IInitializerData
{
    public Task<TokenRespondeDTO> Login(LoginRequestDTO request);

    public Task<MessageRespondDTO> Register(RegisterRequestDTO request);

    // Future service here
}