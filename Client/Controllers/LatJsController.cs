﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LatJsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
