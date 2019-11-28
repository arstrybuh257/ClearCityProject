using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClearCity.Models
{
    public class Plan
    {
        [Key]
        public int PlanId { get; set; }
        [Required]
        public int HouseId { get; set; }
        [Required]
        public int TeamId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public virtual House House { get; set; }
        public virtual Team Team { get; set; }
    }
}