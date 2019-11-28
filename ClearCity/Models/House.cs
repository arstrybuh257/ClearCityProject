using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class House
    {
        public int HouseId { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public int AmountOfCans { get; set; }
        [Required]
        public int MicrodistrictId { get; set; }

        public virtual Microdistrict Microdistrict { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
    }
}