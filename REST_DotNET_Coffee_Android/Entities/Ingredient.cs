using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
[Table("ingredients")]
public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public double AddPrice { get; set; }

    public string Type { get; set; } = string.Empty;
}
