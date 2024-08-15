public class ProductRequestDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public EProductType Type { get; set; }

    public double BasePrice { get; set; }

    public int Quantities { get; set; }

    public bool Active { get; set; }

    public int CategoryId { get; set; }

    public string AvatarUrl { get; set; } = String.Empty;
}