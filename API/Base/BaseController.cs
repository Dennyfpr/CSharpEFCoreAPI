using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public ActionResult Post(Entity entity)
        {
            try
            {
                var insert = repository.Insert(entity);
                if (insert == 0)
                {
                    return BadRequest("Data tidak dapat dimasukkan");
                }
                else if (insert == 1)
                {
                    return Ok("Data berhasil dimasukkan");
                }
                return BadRequest("Data gagal dimasukkan");
            }
            catch
            {
                return BadRequest("Data gagal dimasukkan");
            }
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var getAll = repository.Get();
            if (getAll == null)
            {
                return BadRequest("Tidak ada data yang dapat ditemukan");
            }
            return Ok(getAll);
        }

        [HttpGet("{id}")]
        public ActionResult Get(Key id)
        {
            var getById = repository.Get(id);
            if (getById == null)
            {
                return BadRequest("Data tidak dapat ditemukan");
            }
            return Ok(repository.Get(id));
        }

        [HttpPut]
        public ActionResult Update(Entity entity)
        {
            try
            {
                var update = repository.Update(entity);
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

        [HttpDelete("{id}")]
        public ActionResult Delete(Key id)
        {
            try
            {
                var delete = repository.Delete(id);
                if (delete != 0)
                {
                    return Ok("Data berhasil dihapus");
                }
                return BadRequest($"ID {id} tidak dapat ditemukan");
            }
            catch
            {
                return BadRequest("Aksi hapus data gagal dilakukan");
            }
        }
    }
}
