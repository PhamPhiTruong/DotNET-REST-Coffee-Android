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
        public async Task<ActionResult<TokenRespondeDTO>> Login([FromBody] LoginRequestDTO request)
        {
            return Ok(await _userService.Login(request));
        }

        // POST: Register request
        [HttpPost("register/")]
        public async Task<ActionResult<MessageRespondDTO>> Register([FromBody] RegisterRequestDTO regRequest)
        {
            return Ok(await _userService.Register(regRequest));
        }
    }
}
