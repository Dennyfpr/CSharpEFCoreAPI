using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class getAllVM
    {
        public string nik { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public DateTime birthDate { get; set; }
        public int salary { get; set; }
        public string gpa { get; set; }
        public string degree { get; set; }
        public string universityName { get; set; }
    }
}

// "nik":"2022001","fullName":"Juna Kaiser","phone":"08111111111","gender":"Male","email":"junkai@gmail.com",
// "birthDate":"1999-01-01T00:00:00","salary":900,"education_Id":1,"gpa":"4.00","degree":"S1","universityName":"Universitas Telkom"