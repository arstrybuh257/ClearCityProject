using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class Microdistrict
    {        
        public int MicrodistrictId { get; set; }
        [Required]
        public string MicrodistrictName { get; set; }
        [Required]
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<House> Houses { get; set; }
    }
}