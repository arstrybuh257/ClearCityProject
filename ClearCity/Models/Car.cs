using System;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class Car
    {
        public int CarId { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Number { get; set; }
        public DateTime? DateOfRelease { get; set; }
        public DateTime? DateOfLastInspection { get; set; }
        public int? TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}