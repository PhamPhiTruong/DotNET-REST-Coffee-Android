using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("user_details")]
    public class UserDetail
    {

        [Key]
        public int Id { get; set; }

        public int Expired {  get; set; }

        public int Enable { get; set; }

    }
}
