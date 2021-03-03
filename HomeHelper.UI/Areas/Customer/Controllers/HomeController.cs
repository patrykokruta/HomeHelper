using HomeHelper.SDK.HttpControls;
using Microsoft.AspNetCore.Mvc;

namespace HomeHelper.UI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ISwitch _sw;

        public HomeController(ISwitch sw)
        {
            _sw = sw;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}