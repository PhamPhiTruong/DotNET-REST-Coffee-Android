using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("cart_items")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        public Cart Cart { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
