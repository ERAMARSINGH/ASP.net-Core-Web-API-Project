using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestDemo.Data.Entities
{
  public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public DateTime EmpDOB { get; set; }

        public double EmpSalary { get; set; }

        public string EmpAddress { get; set; }
    }
}
