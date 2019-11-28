using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClearCity.Models
{
    public class Team
    {
        public int TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
    }
}