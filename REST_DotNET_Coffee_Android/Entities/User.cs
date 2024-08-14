using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_DotNET_Coffee_Android.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }

        [ForeignKey("UserInfo")]
        public int infoId { get; set; }
        public UserInfo userInfo { get; set; }

        [ForeignKey("UserDetail")]
        public int detailId { get; set; }
        public UserDetail userDetail { get; set; }

        public string avatar {  get; set; }
    }
}
