using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using REST_DotNET_Coffee_Android.Entities;

#nullable disable

public class UserInfoServiceImpl : AService<UserInfo>, IUserInfoService
{
    public UserInfoServiceImpl(ApplicationDbContext context, ILogger<UserInfoServiceImpl> logger) : base(context, logger)
    {
    }

    public void Initialize()
    {
        if (!_context.UserInfos.Any())
        {
            var userInfos = LoadUserInfosFromFile();

            if (userInfos != null)
            {
                _context.UserInfos.AddRange(userInfos);
                _context.SaveChanges();
                _logger.LogInformation("Inserted userInfos from file.");
            }
        }
    }

    private List<UserInfo> LoadUserInfosFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\users.json");

            JArray jArray = JArray.Parse(json);

            var userInfos = jArray.Select(u => new UserInfo
            {
                FirstName = u["details"]?["first_name"]?.ToString(),
                LastName = u["details"]?["last_name"]?.ToString(),
                Gender = u["details"]?["gender"]?.ToString(),
                Phone = u["details"]?["phone"]?.ToString()
            })
                .ToList();

            return userInfos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading UserInfo from file.");
            return null;
        }
    }
}