using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsRolesController : BaseController<AccountRole, AccountRoleRepository, int>
    {
        private readonly AccountRoleRepository accountRoleRepository;
        public AccountsRolesController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }

        [Authorize(Roles = "Director")]
        [HttpPatch("signmgr/{NIK}")]
        public ActionResult SignManager(string NIK)
        {
            try
            {
                var signMgr = accountRoleRepository.SignManager(NIK);
                if (signMgr == 0)
                {
                    return BadRequest("Tidak terjadi perubahan wewenang");
                }
                else if (signMgr >= 1)
                {
                    return Ok($"{NIK} telah diberikan wewenang Manager!");
                }
                return BadRequest("Gagal mengubah wewenang");
            }
            catch
            {
                return BadRequest("Error, Terjadi kesalahan!");
            }
        }
    }
}