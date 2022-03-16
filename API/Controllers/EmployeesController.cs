using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public IConfiguration _configuration;
        public EmployeesController(EmployeeRepository employeeRepository, IConfiguration configuration) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            this._configuration = configuration;
        }

        [Authorize(Roles = "Director, Manager")]
        [HttpGet("masterdata")]
        public ActionResult GetMasterEmployeeData()
        {
            try
            {               
                var mEmpData = employeeRepository.GetMasterEmployeeData();
                if (mEmpData == null)
                {
                    return BadRequest("Tabel kosong atau tidak ditemukan");
                } else if (mEmpData != null)
                {
                    return Ok(mEmpData);
                }
                return BadRequest("Gagal mengambil data!");
            }
            catch
            {
                return BadRequest("Error, Terjadi kesalahan!");
            }
        }

        [HttpGet("TestCORS")]
        [EnableCors("AllowOrigin")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS berhasil");
        }
    }
}
