using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string avatar { get; set; }

        // Các thuộc tính điều hướng
        public UserInfo infoId { get; set; }

        public UserDetail detailId { get; set; }
    }
}