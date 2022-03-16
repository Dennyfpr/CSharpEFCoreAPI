using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OldEmployeesController : ControllerBase
    {
        private readonly OldEmployeeRepository employeeRepository;
        public OldEmployeesController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            try
            {
                var insert = employeeRepository.Insert(employee);
                if (insert == 0)
                {
                    return BadRequest("Data tidak dapat dimasukkan");
                } else if (insert == 1)
                {
                    return Ok("Data berhasil dimasukkan");
                }
                else if (insert == 101)
                {
                    return BadRequest("Gagal, Email telah terpakai");
                }
                else if (insert == 102)
                {
                    return BadRequest("Gagal, Nomor Telepon telah terpakai");
                }
                return BadRequest("Data gagal dimasukkan");
            }
            catch
            {
                return BadRequest("Data tidak dapat dimasukkan");
            }
        }

        [HttpGet]
        public ActionResult GET()
        {
            var getAll = employeeRepository.Get();
            if (getAll == null)
            {
                return BadRequest("Tidak ada data yang dapat ditemukan");
            }
            return Ok(getAll);
        }

        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            var getById = employeeRepository.Get(NIK);
            if (getById == null)
            {
                return BadRequest("Data tidak dapat ditemukan");
            }
            return Ok(employeeRepository.Get(NIK));
        }

        [HttpPut]
        public ActionResult Update(string NIK, Employee employee)
        {
            try
            {
                var update = employeeRepository.Update(employee);
                if (update != 0)
                {
                    return Ok("Data berhasil diperbaharui");
                }
                return BadRequest("Aksi memperbaharui data gagal dilakukan");
            }
            catch
            {
                return BadRequest("Aksi memperbaharui data gagal dilakukan");
            }
        }

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                var delete = employeeRepository.Delete(NIK);
                if (delete != 0)
                {
                    return Ok("Data berhasil dihapus");
                }
                return BadRequest($"NIK {NIK} tidak dapat ditemukan");
            }
            catch
            {
                return BadRequest("Aksi hapus data gagal dilakukan");
            }
        }
    }
}
