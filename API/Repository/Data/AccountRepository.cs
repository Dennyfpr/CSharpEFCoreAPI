using API.Context;
using API.Models;
using API.ViewModel;
using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        // Constructor
        private readonly MyContext myContext;
        public IConfiguration _configuration;
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this.myContext = myContext;
            this._configuration = configuration;
        }

        // Hashing
        public static string GetSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
        public static string HashPass(string pw)
        {
            return BCrypt.Net.BCrypt.HashPassword(pw, GetSalt());
        }
        public static bool VerifyPass(string pw, string cHash)
        {
            return BCrypt.Net.BCrypt.Verify(pw, cHash); ;
        }

        // Support
        public AccountRoleVM GetUser(LoginVM loginVM)
        {
            var user = (from e in myContext.Employees
                        join a in myContext.Accounts on e.NIK equals a.NIK
                        join ar in myContext.AccountsRoles on a.NIK equals ar.NIK
                        join r in myContext.Roles on ar.Role_Id equals r.Id
                        where e.Email == loginVM.Email
                        select r.Name).ToArray();
            
            AccountRoleVM uRoles = new AccountRoleVM
            {
                Email = loginVM.Email,
                Roles = user
            };

            return uRoles;
        }

        // Methods
        public int Register(RegisterVM registerVM)
        {
            string nik;
            int eduId;
            bool cekEmail = myContext.Employees.Where(e => e.Email == registerVM.Email).SingleOrDefault<Employee>() == null;
            bool cekTelp = myContext.Employees.Where(e => e.Phone == registerVM.Phone).SingleOrDefault<Employee>() == null;

            //Auto Inc NIK
            if (myContext.Employees.ToList().Count() == 0)
            {
                nik = DateTime.Now.ToString("yyyy") + "001";
            }
            else
            {
                int lastNik = int.Parse(myContext.Employees.OrderBy(e => e.NIK).LastOrDefault().NIK);
                lastNik++;
                nik = lastNik.ToString();
            }

            //Check Uniqueness Then Insert Data
            if (cekEmail)
            {
                if (cekTelp)
                {
                    //Insert Data to Temp Emp Tb
                    Employee emp = new Employee
                    {
                        NIK = nik,
                        FirstName = registerVM.FirstName,
                        LastName = registerVM.LastName,
                        Phone = registerVM.Phone,
                        BirthDate = registerVM.BirthDate,
                        Salary = registerVM.Salary,
                        Email = registerVM.Email,
                        Gender = registerVM.Gender
                    };
                    //Insert Data to Db Emp Tb
                    myContext.Employees.Add(emp);

                    //Insert Data to Temp Acc Tb
                    Account acc = new Account
                    {
                        NIK = nik,
                        Password = HashPass(registerVM.Password)
                    };
                    //Insert Data to Db Acc Tb
                    myContext.Accounts.Add(acc);

                    //Insert Data to Temp AcR Tb
                    AccountRole acr = new AccountRole
                    {
                        NIK = nik,
                        Role_Id = 3
                    };
                    //Insert Data to Db AcR Tb
                    myContext.AccountsRoles.Add(acr);

                    //Insert Data to Temp Edu Tb
                    var eduIdCheck = myContext.Educations.FirstOrDefault(e => e.Degree == registerVM.Degree && e.GPA == registerVM.GPA && e.University_Id == registerVM.University_Id);
                    if (eduIdCheck != null)
                    {
                        eduId = eduIdCheck.Id;
                        //Insert Data to Temp Pfl Tb
                        Profiling pfl = new Profiling
                        {
                            NIK = nik,
                            Education_Id = eduId
                        };
                        //Insert Data to Db Pfl Tb
                        myContext.Profilings.Add(pfl);
                    }
                    else
                    {
                        Education edu = new Education
                        {
                            Degree = registerVM.Degree,
                            GPA = registerVM.GPA,
                            University_Id = registerVM.University_Id
                        };
                        //Insert Data to Db Edu Tb
                        myContext.Educations.Add(edu);
                        myContext.SaveChanges();

                        Profiling pfl = new Profiling
                        {
                            NIK = nik,
                            Education_Id = edu.Id
                        };
                        //Insert Data to Db Pfl Tb
                        myContext.Profilings.Add(pfl);
                    }
                }
                else
                {
                    return 102;
                }
                
            } else
            {
                return 101;
            }
            
            //Save changes to database
            var result = myContext.SaveChanges();
            return result;
        }
        public int Login(LoginVM loginVM)
        {   
            var authEm = myContext.Employees.Where(e => e.Email == loginVM.Email).FirstOrDefault<Employee>();
            if (authEm != null)
            {
                var password = myContext.Accounts.Where(a => a.NIK == authEm.NIK).FirstOrDefault<Account>().Password;               
                var authPw = VerifyPass(loginVM.Password, password);
                if (authPw)
                {
                    return 1;
                }
                else
                {
                    return 102;
                }
            } 
            else
            {
                return 101;
            }
        }
        public int ForgotPassword(ForgotPasswordVM fPassVM)
        {
            var authEm = myContext.Employees.Where(e => e.Email == fPassVM.Email).FirstOrDefault<Employee>();
            if (authEm != null)
            {
                Random r = new Random();
                int otp = r.Next(100000, 999999);

                var acc = new Account
                {
                    NIK = authEm.NIK,
                    OTP = otp,
                    ExpiredToken = DateTime.Now.AddMinutes(5),
                    IsUsed = false
                };
                myContext.Accounts.Attach(acc);
                myContext.Entry(acc).Property(a => a.OTP).IsModified = true;
                myContext.Entry(acc).Property(a => a.ExpiredToken).IsModified = true;
                myContext.Entry(acc).Property(a => a.IsUsed).IsModified = true;

                var result = myContext.SaveChanges();  
                
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Belajar API", "Work321Inc@gmail.com"));
                email.To.Add(MailboxAddress.Parse(authEm.Email));
                email.Subject = "OTP Forgot Password";
                email.Body = new TextPart("Plain") { Text = $"Kode OTP Anda: {acc.OTP}" };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("Work321Inc@gmail.com", "AR01990201");
                smtp.Send(email);
                smtp.Disconnect(true);

                return result;
            }
            else
            {
                return 101;
            }
        }
        public int ChangePassword(ChangePasswordVM cPassVM)
        {
            var authEm = myContext.Employees.AsNoTracking().Where(e => e.Email == cPassVM.Email).FirstOrDefault<Employee>();
            if (authEm != null)
            {
                var getAcc = myContext.Accounts.AsNoTracking().Where(a => a.NIK == authEm.NIK).FirstOrDefault<Account>();
                var authOTP = getAcc.OTP == cPassVM.OTP;
                if (authOTP)
                {
                    bool authExp = getAcc.ExpiredToken > DateTime.Now;
                    bool authUsed = getAcc.IsUsed == false;
                    if(authExp && authUsed)
                    {
                        var authPass = cPassVM.Password == cPassVM.CPassword;
                        if (authPass)
                        {
                            Account ac = new Account
                            {
                                NIK = getAcc.NIK,
                                Password = HashPass(cPassVM.Password),
                                IsUsed = true
                            };
                            myContext.Accounts.Attach(ac);
                            myContext.Entry(ac).Property(a => a.Password).IsModified = true;
                            myContext.Entry(ac).Property(a => a.IsUsed).IsModified = true;

                            var result = myContext.SaveChanges();
                            return result;
                        }
                        else
                        {
                            return 104;
                        }
                    }
                    else
                    {
                        return 103;
                    }
                } else
                {
                    return 102;
                }
            }
            return 101;
        }
    }
}

/*
while (pass.Length < 7 || pass.Length > 16 || pass.Any(char.IsUpper) == false || pass.Any(char.IsLower) == false || pass.Any(char.IsNumber) == false)
            {
                Console.WriteLine("\nPassword must have at least 8 characters and maximum of 15 characters\n with at least one Capital letter, at least one lower case letter and at least one number.");
                Console.Write("Input : "); pass = Console.ReadLine();
            }
*/