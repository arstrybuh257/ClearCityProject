using System;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
    }
}