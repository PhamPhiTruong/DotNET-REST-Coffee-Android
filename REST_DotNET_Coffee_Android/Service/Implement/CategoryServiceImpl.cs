using Newtonsoft.Json;

#nullable disable

public class CategoryServiceImpl : AService<Category>, ICategoryService
{
    public CategoryServiceImpl(ApplicationDbContext context, ILogger<CategoryServiceImpl> logger) : base(context, logger) { }

    // Import data
    public void Initialize()
    {
        if (!_context.Categories.Any())
        {
            var categories = LoadCategoryFromFile();

            if (categories != null)
            {
                _context.AddRange(categories);
                _context.SaveChanges();
                _logger.LogInformation("Inserted category from file.");
            }
        }
    }

    private List<Category> LoadCategoryFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\categories.json");

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);

            var categories = new List<Category>();

            foreach (var item in data)
            {
                var category = new Category
                {
                    Id = Convert.ToInt32(item["id"]),
                    Name = item["name"].ToString(),
                    Active = Convert.ToBoolean(item["active"]),
                    AvatarURL = item["avatar"].ToString(),
                    Type = item["type"].ToString()
                };

                categories.Add(category);
            }

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product from file.");

            return null;
        }
    }

}