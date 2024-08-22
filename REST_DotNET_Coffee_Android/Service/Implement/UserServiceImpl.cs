using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using REST_DotNET_Coffee_Android.Entities;

#nullable disable

public class UserServiceImpl : AService<User>, IUserService
{
    public UserServiceImpl(ApplicationDbContext context, ILogger<IUserService> logger) : base(context, logger)
    {
    }

    public void Initialize()
    {
        if (!_context.Users.Any())
        {
            var userss = LoadUsersFromFile();

            if (userss != null)
            {
                _context.Users.AddRange(userss);
                _context.SaveChanges();
                _logger.LogInformation("Inserted userDetails from file.");
            }
        }
    }

    private List<User> LoadUsersFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\users.json");

            JArray jArray = JArray.Parse(json);
            Console.WriteLine(jArray.ToString());

            var users = new List<User>();

            foreach (var u in jArray)
            {
                var username = u["username"]?.ToString();

                var password = u["password"]?.ToString();

                var email = u["details"]?["email"]?.ToString();

                var avatar = u["details"]?["img"]?.ToString();

                var phone = u["details"]?["phone"]?.ToString();

                // Kiểm tra và tìm UserInfo
                if (string.IsNullOrEmpty(phone))
                {
                    _logger.LogWarning("Phone number is missing in the data.");
                    continue; // Bỏ qua người dùng này nếu không có số điện thoại
                }

                var userInfo = _context.UserInfos
                    .FirstOrDefault(ui => ui.Phone == phone);

                if (userInfo == null)
                {
                    _logger.LogWarning($"No UserInfo found for phone: {phone}");
                    continue; // Bỏ qua người dùng này nếu không tìm thấy thông tin
                }

                // Tìm UserDetail dựa trên tiêu chí cụ thể
                var userDetail = _context.UserDetails
                    .FirstOrDefault(ud => ud.Expired == 0 && ud.Enable == 1);

                if (userDetail == null)
                {
                    _logger.LogWarning("No UserDetail found with the specified criteria.");
                    continue; // Bỏ qua người dùng này nếu không tìm thấy chi tiết
                }

                // Tạo đối tượng User và thêm vào danh sách
                var user = new User
                {
                    UserName = username,
                    Password = password,
                    Email = email,
                    InfoId = userInfo.Id,
                    DetailId = userDetail.Id,
                    Avatar = avatar
                };

                users.Add(user);
            }

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users from file.");
            return null;
        }
    }

    /// <summary>
    /// Authenticates the user and generates a JWT token.
    /// </summary>
    /// <param name="request">The login request containing username and password.</param>
    /// <returns>
    /// Returns a JWT token if authentication is successful.
    /// If the authentication fails, returns a 401 Unauthorized status.
    /// </returns>
    /// <response code="200">Returns the JWT token and user details.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="401">If the username or password is incorrect.</response>
    /// <response code="500">If an internal server error occurs.</response>
    public async Task<TokenRespondeDTO> Login(LoginRequestDTO request)
    {

        if (request is null)
        {
            throw new InvalidRequest();
        }

        string Email = request.Email;

        string Password = request.Password;

        var user = await _context.Users
                            .Where(u => u.Email == Email && u.Password == Password)
                            .FirstOrDefaultAsync<User>();

        if (user is null)
        {
            return new TokenRespondeDTO()
            {
                Message = "Email and password are not correct or not already register. Please check again."
            };
        }

        var userDetail = await _context.UserDetails
                                    .Where(u => u.Id == user.DetailId)
                                    .FirstOrDefaultAsync<UserDetail>();

        if (userDetail is null)
        {
            return new TokenRespondeDTO()
            {
                Message = "User suspended."
            };
        }

        if (userDetail.Enable == 0 || userDetail.Expired == 1)
        {
            return new TokenRespondeDTO()
            {
                Message = $"User {user.UserName} with email {user.Email} has been disabled or expired."
            };
        }

        return new TokenRespondeDTO
        {
            Message = "Login successfully.",
            Token = user.Id + ""
        };
    }

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    /// <param name="request">The registration request containing user details like username, password, and email.</param>
    /// <returns>
    /// Returns a 201 Created status if the registration is successful.
    /// If the registration fails due to a conflict (e.g., username already exists), returns a 409 Conflict status.
    /// </returns>
    /// <response code="201">Returns if the user is successfully registered.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="409">If the username or email is already in use.</response>
    /// <response code="500">If an internal server error occurs.</response>
    public async Task<ActionResult<MessageRespondDTO>> Register(RegisterRequestDTO request)
    {
        if (request is null)
        {
            throw new InvalidRequest();
        }

        string Email = request.Email;

        string Password = request.Password;

        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            throw new InvalidRequest();
        }

        var existed = await _context.Users
                                .Where(u => u.Email == Email)
                                .ToListAsync();

        if (existed.Count > 0)
        {
            return new MessageRespondDTO()
            {
                Message = $"Error code: 400. Given email {Email} already in used."
            };
        }

        UserInfo userInfo = new UserInfo()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Phone = request.Phone
        };

        UserDetail userDetail = new UserDetail()
        {
            Enable = 1,
            Expired = 0
        };

        User user = new User()
        {
            UserInfo = userInfo,
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password,
            UserDetail = userDetail
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return new MessageRespondDTO()
        {
            Message = "Register successfully, please login."
        };
    }

}