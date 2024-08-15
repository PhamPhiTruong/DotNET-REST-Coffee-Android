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

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);

            var userDetails = new List<UserDetail>();

            foreach (var item in data) 
            {
                var userDetail = new UserDetail
                {
                    Expired = 0, // Hoặc lấy từ dữ liệu nếu có
                    Enable = 1   // Hoặc lấy từ dữ liệu nếu có 
                };

                userDetails.Add(userDetail);
            }
            
            return userDetails;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading UserDetail from file.");
            return null;
        }
    }
}
