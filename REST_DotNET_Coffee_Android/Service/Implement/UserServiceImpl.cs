using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using REST_DotNET_Coffee_Android.Entities;

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

            // Giả sử bạn đã có UserInfo và UserDetail trong cơ sở dữ liệu và lấy chúng từ đó
            var userInfos = _context.UserInfos.ToList();

            var userDetails = _context.UserDetails.ToList();

            var users = new List<User>();

            foreach (var u in jArray)
            {

                var username = u["username"]?.ToString();

                var password = u["password"]?.ToString();

                var email = u["details"]?["email"]?.ToString();

                var avatar = u["details"]?["img"]?.ToString();

                // Lấy UserInfo và UserDetail từ cơ sở dữ liệu
                var userInfo = userInfos.FirstOrDefault(); // Hoặc chọn UserInfo cụ thể nếu cần

                var userDetail = userDetails.FirstOrDefault(); // Hoặc chọn UserDetail cụ thể nếu cần

                var user = new User
                {
                    userName = username,
                    password = password,
                    email = email,
                    avatar = avatar,
                    infoId = userInfo,
                    detailId = userDetail
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
}
