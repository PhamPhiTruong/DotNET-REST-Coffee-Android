using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
[Table("order_items")]
public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    public Order Order { get; set; }

    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; }

    public int Quantities { get; set; }

}