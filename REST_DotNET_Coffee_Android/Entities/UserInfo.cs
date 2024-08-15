using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_DotNET_Coffee_Android.Entities
{
    [Table("user_infos")]
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public String Phone { get; set; } = string.Empty;

    }
}
