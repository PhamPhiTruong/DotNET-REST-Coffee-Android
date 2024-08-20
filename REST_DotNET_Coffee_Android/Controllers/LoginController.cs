using Microsoft.AspNetCore.Mvc;

namespace REST_DotNET_Coffee_Android.Controllers
{
    [ApiController]
    [Route("v1/")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: Login request
        [HttpPost("login/")]
        public async Task<TokenRespondeDTO> Login([FromBody] LoginRequestDTO request)
        {
            return await _userService.Login(request);
        }

        // POST: Register request
        [HttpPost("register/")]
        public async Task<MessageRespondDTO> Register([FromBody] RegisterRequestDTO regRequest)
        {
            return await _userService.Register(regRequest);
        }
    }
}
