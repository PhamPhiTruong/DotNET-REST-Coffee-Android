using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using REST_DotNET_Coffee_Android.Entities;
#nullable disable

public class UserDetailServiceImpl : AService<UserDetail>, IUserDetailService
{
    public UserDetailServiceImpl(ApplicationDbContext context, ILogger<UserDetailServiceImpl> logger) : base(context, logger)
    {
    }

    public void Initialize()
    {
        if (!_context.UserDetails.Any())
        {
            var userDetails = LoadUserDetailsFromFile();

            if (userDetails != null)
            {
                _context.UserDetails.AddRange(userDetails);
                _context.SaveChanges();
                _logger.LogInformation("Inserted userDetails from file.");
            }
        }
    }

    private List<UserDetail> LoadUserDetailsFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\users.json");
            JArray jArray = JArray.Parse(json);

            var userDetails = jArray.Select(u => new UserDetail
            {
                expired = 0, // Hoặc lấy từ dữ liệu nếu có
                enable = 1   // Hoặc lấy từ dữ liệu nếu có
            }).ToList();

            return userDetails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading UserDetail from file.");
            return null;
        }
    }
}
