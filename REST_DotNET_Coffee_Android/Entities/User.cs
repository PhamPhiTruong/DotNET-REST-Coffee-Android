using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("users")]
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
        public UserInfo UserInfo { get; set; }

        [ForeignKey("UserDetail")]
        public int detailId { get; set; }
        public UserDetail UserDetail { get; set; }
        
        public string avatar { get; set; }
        
    }
}