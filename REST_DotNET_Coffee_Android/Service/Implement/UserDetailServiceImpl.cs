using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var usersDetails = LoadUserDetailsFromFile();

            if (usersDetails != null)
            {
                _context.UserDetails.AddRange(usersDetails);
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
            Console.WriteLine(jArray);
            JArray detailUserArray = new JArray(
                jArray.Select( du => new JObject
                {
                    {"expired", 0 },
                    {"enable", 1 }
                })
            );
            json = detailUserArray.ToString();
            return JsonConvert.DeserializeObject<List<UserDetail>>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading userDetail from file.");

            return null;
        }
    }
}

