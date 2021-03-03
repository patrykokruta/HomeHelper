using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}