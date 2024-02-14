using System.ComponentModel.DataAnnotations;

namespace EisenringDamianLB_295.Models
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

}
