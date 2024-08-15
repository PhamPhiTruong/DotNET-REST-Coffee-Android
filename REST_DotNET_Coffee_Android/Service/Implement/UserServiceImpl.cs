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

    // Login
    // Return message login failure if login unsuccessful.
    // Return token with message if login successful.
    public async Task<ActionResult<TokenRespondeDTO>> Login(LoginRequestDTO request)
    {

        if (request is null)
        {
            return new BadRequestResult();
        }

        string Email = request.Email;

        string Password = request.Password;

        var user = await _context.Users
                            .Where(u => u.Email == Email && u.Password == Password)
                            .ToListAsync();

        if (user is null || user.Count == 0)
        {
            return new TokenRespondeDTO()
            {
                Message = "Email and password are not correct or not already register. Please check again."
            };
        }

        return new TokenRespondeDTO
        {
            Message = "Login successfully.",
            Token = "Feature on working, please come back later."
        };
    }

    // Register
    // Typically add new user to users table
    // IF user ALREADY registered, return message $"User with email {Email} already registered."
    // IF user NOT ALREADY registered, add new user to users table, return message "Register successfully."
    public async Task<ActionResult<MessageRespondDTO>> Register(RegisterRequestDTO request)
    {
        if (request is null)
        {
            return new BadRequestResult();
        }

        string Email = request.Email;

        string Password = request.Password;

        if (string.IsNullOrEmpty(Email))
        {
            return new BadRequestResult();
        }

        var existed = await _context.Users
                                .Where(u => u.Email == Email)
                                .ToListAsync();

        if (existed.Count > 0)
        {
            return new MessageRespondDTO()
            {
                Message = $"User with email {Email} already registered."
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