using System.ComponentModel.DataAnnotations;

public class PaymentMethod
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

}
