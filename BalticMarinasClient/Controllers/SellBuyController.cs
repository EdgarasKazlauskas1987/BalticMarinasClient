﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BalticMarinasClient.Controllers
{
    public class SellBuyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}