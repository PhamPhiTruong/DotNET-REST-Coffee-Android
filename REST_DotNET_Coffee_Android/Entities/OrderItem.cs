using System.ComponentModel.DataAnnotations;

public class OrderItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    public Order Order { get; set; } = new Order();

    [Required]
    public int ProductId { get; set; }

    public Product Product { get; set; } = new Product();

    public int Quantities { get; set; }

}