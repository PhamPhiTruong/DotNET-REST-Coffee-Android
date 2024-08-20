using Microsoft.AspNetCore.Mvc;

public interface IUserService : IInitializerData
{
    public Task<TokenRespondeDTO> Login(LoginRequestDTO request);

    public Task<ActionResult<MessageRespondDTO>> Register(RegisterRequestDTO request);

    // Future service here
}