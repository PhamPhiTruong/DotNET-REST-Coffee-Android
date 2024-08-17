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
        if (ingredients is null) throw new IngredientNullException();

        List<IngredientRespondeDTO> responde = new List<IngredientRespondeDTO>();

        ingredients.ForEach(i =>
        {
            responde.Add(ToDTO(i));
        });

        return responde;
    }

    private IngredientRespondeDTO ToDTO(Ingredient ingredient)
    {
        if (ingredient is null) throw new IngredientNullException();

        return new IngredientRespondeDTO
        {
            Name = ingredient.Name,
            AddPrice = ingredient.AddPrice,
            Type = ingredient.Type
        };
    }

    //
    // GET: return all ingredients
    // Summary:
    //     Finds all ingredients. A query is made to the database for ingredients, if found, is
    //     attached to the context and returned. If no ingredient is found, then null is returned.
    //
    //
    // Returns:
    //     The ingredients found, or null.
    //
    // Remarks:
    //     See Using GetIngredients for more information and examples.
    public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredients()
    {
        var ingredients = await _context.Ingredients.ToListAsync<Ingredient>();

        if (ingredients is null || ingredients.Count <= 0)
        {
            return null;
        }

        return ToListDTO(ingredients);
    }

    //
    // Summary:
    //     Finds an ingredient with the given primary key values. If an ingredient with the given
    //     primary key values is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for an ingredient with the given primary key values and this ingredient, if found, is
    //     attached to the context and returned. If no ingredient is found, then null is returned.
    //
    //
    // Parameters:
    //   keyValues:
    //     The values of the primary key for the ingredient to be found.
    //
    // Returns:
    //     The ingredient found, or null.
    //
    // Remarks:
    //     See Using GetIngredients and GetIngredientById for more information and examples.
    public async Task<ActionResult<IngredientRespondeDTO>> GetIngredientById(int id)
    {
        if (id <= 0) throw new InvalidIdException();

        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient is null) { return null; }

        return ToDTO(ingredient);
    }

    //
    // Summary:
    //     Finds an ingredient with the given enum type. If an ingredient with the given
    //     enum type is being tracked by the context, then it is returned immediately
    //     without making a request to the database. Otherwise, a query is made to the database
    //     for an ingredient with the given enum type and this ingredient, if found, is
    //     attached to the context and returned. If no ingredient is found, then null is returned.
    //
    //
    // Parameters:
    //   keyValues:
    //     The enums of type for the ingredient to be found.
    //
    // Returns:
    //     The ingredient found, or null.
    //
    // Remarks:
    //     See Using GetIngredients and GetIngredientById for more information and examples.
    public async Task<ActionResult<List<IngredientRespondeDTO>>> GetIngredientByType(EIngredientType type)
    {

        if (type <= 0)
        {
            throw new IngredientException($"Invalid input enum type '{type}'. An type enum should not be smaller than 0.");
        }

        string typeStr = IngredientTypeExtension.ToString(type);

        var ingredients = await _context.Ingredients
                            .Where<Ingredient>(i => i.Type == typeStr)
                            .ToListAsync();

        if (ingredients is null || ingredients.Count <= 0) return null;

        return ToListDTO(ingredients);
    }
}