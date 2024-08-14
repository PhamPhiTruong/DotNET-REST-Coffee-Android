using System.ComponentModel.DataAnnotations;

namespace REST_DotNET_Coffee_Android.Entities
{
    public class UserDetail
    {
        [Key]
        public int id { get; set; }
        public int expired {  get; set; }
        public int enable { get; set; }

    }
}
