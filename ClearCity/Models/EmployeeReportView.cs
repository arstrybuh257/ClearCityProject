using System.Collections.Generic;

namespace ClearCity.Models
{
    public class EmployeeReportView
    {
        public string TeamName;
        public List<Employee> Employees { get; set; }
    }
}