using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("user_details")]
    public class UserDetail
    {
        [Key]
        public int id { get; set; }
        public int expired {  get; set; }
        public int enable { get; set; }

    }
}
