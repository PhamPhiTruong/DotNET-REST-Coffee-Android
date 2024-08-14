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
                    .FirstOrDefault(ui => ui.phone == phone);

                if (userInfo == null)
                {
                    _logger.LogWarning($"No UserInfo found for phone: {phone}");
                    continue; // Bỏ qua người dùng này nếu không tìm thấy thông tin
                }

                // Tìm UserDetail dựa trên tiêu chí cụ thể
                var userDetail = _context.UserDetails
                    .FirstOrDefault(ud => ud.expired == 0 && ud.enable == 1);

                if (userDetail == null)
                {
                    _logger.LogWarning("No UserDetail found with the specified criteria.");
                    continue; // Bỏ qua người dùng này nếu không tìm thấy chi tiết
                }

                // Tạo đối tượng User và thêm vào danh sách
                var user = new User
                {
                    userName = username,
                    password = password,
                    email = email,
                    infoId = userInfo.id,
                    detailId = userDetail.id,
                    avatar = avatar
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