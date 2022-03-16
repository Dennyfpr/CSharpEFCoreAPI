using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }
        
        public IEnumerable GetMasterEmployeeData()
        {       
            var mEmpDTable = (from e in myContext.Employees join a in myContext.Accounts on e.NIK equals a.NIK
                         join p in myContext.Profilings on a.NIK equals p.NIK
                         join d in myContext.Educations on p.Education_Id equals d.Id
                         join u in myContext.Universities on d.University_Id equals u.Id
                         select new
                         {
                             NIK = e.NIK,
                             FullName = $"{e.FirstName} {e.LastName}",
                             Phone = e.Phone,
                             Gender = e.Gender.ToString(),
                             Email = e.Email,
                             BirthDate = e.BirthDate,
                             Salary = e.Salary,
                             Education_Id = p.Education_Id,
                             GPA = d.GPA,
                             Degree = d.Degree,
                             UniversityName = u.Name
                         }).ToList();

            return mEmpDTable;
        }

    }
}
