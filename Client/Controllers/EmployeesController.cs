using API.Models;
using API.ViewModel;
using Client.Base;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository repository;
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<JsonResult> GetAllProfile()
        {
            var result = await repository.GetAllProfile();
            return Json(result);
        }

        [HttpPost]
        public JsonResult CreateNewProfile([FromBody] RegisterVM newData)
        {
            var result = repository.CreateNewProfile(newData);
            return Json(result);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
