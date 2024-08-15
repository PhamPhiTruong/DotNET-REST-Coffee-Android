using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

[Table("products")]
public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = String.Empty;

    public string Type { get; set; } = String.Empty;

    public double BasePrice { get; set; }

    public int Quantities { get; set; }

    public bool Active { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public string AvatarUrl { get; set; } = String.Empty;

    public HavingIngredient HavingIngredient { get; set; }
}