using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    private List<IngredientRespondeDTO> ToListDTO(List<Ingredient> ingredients)
    {
        List<IngredientRespondeDTO> responde = new List<IngredientRespondeDTO>();

        ingredients.ForEach(i =>
        {
            responde.Add(ToDTO(i));
        });

        return responde;
    }

    private IngredientRespondeDTO ToDTO(Ingredient ingredient)
    {
        return new IngredientRespondeDTO
        {
            Name = ingredient.Name,
            AddPrice = ingredient.AddPrice,
            Type = ingredient.Type
        };
    }

    // GET: return all ingredients
    public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredients()
    {
        var ingredients = await _context.Ingredients.ToListAsync<Ingredient>();

        if (ingredients is null || ingredients.Count <= 0)
        {
            return new BadRequestResult();
        }

        return ToListDTO(ingredients);
    }

    // GET: get product by Id
    public async Task<ActionResult<IngredientRespondeDTO>> GetIngredientById(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient is null) { return new BadRequestResult(); }

        return ToDTO(ingredient);
    }

    // GET: get product by Type
    public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredientByType(EIngredientType type)
    {
        string typeStr = IngredientTypeExtension.ToString(type);

        var ingredients = await _context.Ingredients
                            .Where<Ingredient>(i => i.Type == typeStr)
                            .ToListAsync();

        return ToListDTO(ingredients);
    }
}