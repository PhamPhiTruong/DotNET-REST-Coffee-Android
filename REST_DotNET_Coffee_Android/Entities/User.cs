using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [ForeignKey("UserInfo")]
        public int InfoId { get; set; }

        public UserInfo UserInfo { get; set; }

        [ForeignKey("UserDetail")]
        public int DetailId { get; set; }

        public UserDetail UserDetail { get; set; }
        
        public string Avatar { get; set; }
        
    }
}