using Microsoft.AspNetCore.Mvc;

namespace Home.Controllers
{

    public class HomeController : Controller
    {

        [Route("index")]
        [Route("home")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

    }

}