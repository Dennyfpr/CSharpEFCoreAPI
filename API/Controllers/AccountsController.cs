using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public IConfiguration _configuration;

        public AccountsController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
        }
        
        [HttpPost("register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            try
            {
                var reg = accountRepository.Register(registerVM);
                if (reg == 0)
                {
                    return BadRequest("Data tidak dapat dimasukkan");
                } 
                else if (reg == 101)
                {
                    return BadRequest("Gagal, Email sudah terpakai");
                }
                else if (reg == 102)
                {
                    return BadRequest("Gagal, Nomor Telepon sudah terpakai");
                }
                else if (reg >= 1)
                {
                    return Ok("Data berhasil dimasukkan");
                }
                return BadRequest("Data gagal dimasukkan");
            }
            catch
            {
                return BadRequest("Error, Terjadi kesalahan!");
            }
        }

        [HttpGet("login")]
        public ActionResult Login(LoginVM loginVM)
        {
            try
            {               
                var login = accountRepository.Login(loginVM);
                if (login == 101)
                {
                    return BadRequest("Email tidak ditemukan dalam sistem");
                } 
                else if (login == 102)
                {
                    return BadRequest("Password dan email tidak cocok");
                } else if (login == 1)
                {
                    var user = accountRepository.GetUser(loginVM);

                    var claims = new List<Claim> { new Claim("Email", user.Email)};
                    foreach (string r in user.Roles) { claims.Add(new Claim("roles", r)); }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );
                    var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                    claims.Add(new Claim("TokenSecurity", idtoken.ToString()));

                    return Ok(new { status = HttpStatusCode.OK, idtoken, message = "Login berhasil!" });
                }
                return BadRequest("Gagal login");
            }
            catch
            {
                return BadRequest("Error, Terjadi kesalahan!");
            }
        }

        [HttpPatch("forgot")]
        public ActionResult ForgotPassword(ForgotPasswordVM forgot)
        {
            try
            {
                var forgotp = accountRepository.ForgotPassword(forgot);
                if (forgotp == 101)
                {
                    return BadRequest("Email tidak ditemukan dalam sistem");
                }
                else if (forgotp == 1)
                {
                    return Ok("OTP telah dikirimkan! Silahkan cek Email anda");
                }
                return BadRequest("Gagal mengirimkan OTP");
            }
            catch
            {
                return BadRequest("Error, Terjadi kesalahan!");
            }
        }

        [HttpPatch("change")]
        public ActionResult ChangePaswword(ChangePasswordVM change)
        {
            
                var changep = accountRepository.ChangePassword(change);
                if (changep == 101)
                {
                    return BadRequest("Email tidak ditemukan dalam sistem");
                } 
                else if (changep == 102)
                {
                    return BadRequest("OTP tidak sesuai");
                }
                else if (changep == 103)
                {
                    return BadRequest("OTP telah terpakai atau kadaluarsa");
                }
                else if (changep == 104)
                {
                    return BadRequest("Password dan Confirm Password tidak sama");
                }
                else if (changep == 1)
                {
                    return Ok("Password berhasil diganti");
                }
                return BadRequest("Gagal mengubah password");
            
        }
    }
}
