using System.ComponentModel.DataAnnotations;

#nullable disable

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Type { get; set; }

    public float BasePrice { get; set; }

    public int Quantities { get; set; }

    public bool Active { get; set; }

    public int CategoryId { get; set; }

    public string AvatarUrl { get; set; }

}
