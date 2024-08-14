using System.ComponentModel.DataAnnotations;

namespace REST_DotNET_Coffee_Android.Entities
{
    public class UserInfo
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public int phone { get; set; }
    }
}
