using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityFramwork.Models
{
    public class Detail : Employee
    {
        public List<Employee> Employees { get; set; }

        public String key { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int Age { get; set; }
    }
}