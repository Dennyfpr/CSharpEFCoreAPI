using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext, AccountRole, int>
    {
        private readonly MyContext myContext;
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public int SignManager(string nik)
        {
            bool cekRole = myContext.AccountsRoles.Where(ar => ar.NIK == nik && ar.Role_Id == 2).FirstOrDefault() == null;

            if (cekRole)
            {
                AccountRole acr = new AccountRole
                {
                    NIK = nik,
                    Role_Id = 2
                };
                myContext.AccountsRoles.Add(acr);

                var result = myContext.SaveChanges();
                return result;
            } else
            {
                return 0;
            }
        }
    }
}

//var userId = myContext.AccountsRoles.AsNoTracking().Where(ar => ar.NIK == NIK).FirstOrDefault().Id;
//AccountRole ac = new AccountRole
//{
//    Id = userId,
//    Role_Id = 2
//};
//myContext.AccountsRoles.Attach(ac);
//myContext.Entry(ac).Property(a => a.Role_Id).IsModified = true;