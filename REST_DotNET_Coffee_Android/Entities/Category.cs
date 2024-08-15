using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("categories")]
public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool Active { get; set; }

    [StringLength(500)]
    public string AvatarURL { get; set; } = string.Empty;

    [StringLength(100)]
    public string Type { get; set; } = string.Empty;
}
