using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Stored_Proc
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }

    }
}
