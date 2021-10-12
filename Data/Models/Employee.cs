using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationOfCompanyEmployees.Data.Models
{
    public class Employee
    {
        public Employee(int employeeId, string surname, string name, string middleName, DateTime employmentDate, string position, int companyId)
        {
            EmployeeId = employeeId;
            Surname = surname;
            Name = name;
            MiddleName = middleName;
            EmploymentDate = employmentDate;
            Position = position;
            CompanyId = companyId;
        }

        public int EmployeeId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string Position { get; set; }
        public int CompanyId { get; set; }
    }
}
