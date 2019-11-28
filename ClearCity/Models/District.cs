using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        [Required]
        public string DistrictName { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Population { get; set; }
        [Required]
        public virtual string Manager { get; set; }

        public virtual ICollection<Microdistrict> Microdistricts { get; set; }
    }
}