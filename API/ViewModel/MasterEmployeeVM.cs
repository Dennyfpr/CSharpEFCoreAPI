using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class MasterEmployeeVM
    {
        // NIK, FullName, Phone, Gender, Email, BirthDate, Salary, Education_Id, GPA, Degree, UniversityName
        public string NIK { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public int Education_Id { get; set; }
        public string GPA { get; set; }
        public string Degree { get; set; }
        public string UniversityName { get; set; }
    }

}
