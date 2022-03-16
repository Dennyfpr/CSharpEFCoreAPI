using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class OldEmployeeRepository : IEmployeeRepository
    {
        public int Insert(Employee employee)
        {
            //Inserting Data
            context.Employees.Add(employee);

            //Checking Uniqueness
            var cekEmail = context.Employees.Where(e => e.Email == employee.Email).SingleOrDefault<Employee>();
            var cekTelp = context.Employees.Where(e => e.Phone == employee.Phone).SingleOrDefault<Employee>();
            if (cekEmail != null)
            {
                return 101;
            }
            else if (cekTelp != null)
            {
                return 102;
            }

            //Overwrite NIK with new Autoincrement Format
            if (Get().Count() == 0)
            {
                employee.NIK = DateTime.Now.ToString("yyyy") + "001";
            }
            else
            {
                int lastNik = int.Parse(context.Employees.OrderBy(e => e.NIK).LastOrDefault().NIK);
                lastNik++;
                employee.NIK = lastNik.ToString();
            }

            //Save changes!
            var result = context.SaveChanges();
            return result;
        }

        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            return context.Employees.Find(NIK);
        }

        public int Update(Employee employee)
        {
            context.Entry(employee).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }

        private readonly MyContext context;

        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }
    }
}
