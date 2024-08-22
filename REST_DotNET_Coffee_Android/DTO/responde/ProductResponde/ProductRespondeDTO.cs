#nullable disable

public class ProductRespondeDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public string Type { get; set; } = string.Empty;

    public double BasePrice { get; set; }

    public int Quantities { get; set; }

    public bool Active { get; set; }

    public int CategoryId { get; set; }

    public string AvatarUrl { get; set; } = String.Empty;

    public List<IngredientRespondeDTO> Ingredients { get; set; }
}