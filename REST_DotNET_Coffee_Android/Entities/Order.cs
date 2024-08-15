using REST_DotNET_Coffee_Android.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
[Table("orders")]
public class Order
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; }

    [ForeignKey("PaymentMethod")]
    public int PaymentId { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public double TotalPrice { get; set; }

}
