using Newtonsoft.Json;

#nullable disable

public class IngredientServiceImpl : AService<Ingredient>, IIngredientService
{
    public IngredientServiceImpl(ApplicationDbContext context, ILogger<IngredientServiceImpl> logger) : base(context, logger) { }

    // Import data
    public void Initialize()
    {
        if (!_context.Ingredients.Any())
        {
            var ingredients = LoadIngredientFromFile();

            if (ingredients != null)
            {
                _context.AddRange(ingredients);
                _context.SaveChanges();
                _logger.LogInformation("Inserted Ingredient from file.");
            }
        }
    }

    private List<Ingredient> LoadIngredientFromFile()
    {
        try
        {
            var json = File.ReadAllText("resources\\ingredients.json");

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);

            var ingredients = new List<Ingredient>();

            foreach (var item in data)
            {
                var Ingredient = new Ingredient
                {
                    Id = Convert.ToInt32(item["id"]),
                    Name = item["name"].ToString(),
                    AddPrice = Convert.ToDouble(item["add_price"]),
                    Type = item["type"].ToString()
                };

                ingredients.Add(Ingredient);
            }

            return ingredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading product from file.");

            return null;
        }
    }
}