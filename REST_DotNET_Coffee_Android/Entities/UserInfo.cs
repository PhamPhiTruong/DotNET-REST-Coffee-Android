using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("user_info")]
    public class UserInfo
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public String phone { get; set; }
    }
}
