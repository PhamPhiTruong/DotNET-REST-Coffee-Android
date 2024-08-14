using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categories")]
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; }

    public string AvatarURL { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}
