using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("carts")]
    public class Cart
    {
        [Key]
        public int Id {  get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
