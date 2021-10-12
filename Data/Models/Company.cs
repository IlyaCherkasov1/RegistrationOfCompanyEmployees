using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationOfCompanyEmployees.Data.Models
{
    public class Company
    {
        public Company(int companyId, string name, string legalForm)
        {
            CompanyId = companyId;
            Name = name;
            LegalForm = legalForm;
        }

        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string LegalForm { get; set; }
    }
}
