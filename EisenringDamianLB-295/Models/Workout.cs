using System.ComponentModel.DataAnnotations;

namespace EisenringDamianLB_295.Models
{
    public class Workout
    {
        [Key]
        public int Id { get; set; }
        public int CaloriesBurned { get; set; }
        public string NamePerson { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
    }
}
