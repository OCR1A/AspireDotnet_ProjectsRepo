using Microsoft.AspNetCore.Mvc;

namespace ShopNamespace.Controllers
{

    public class ShopController : Controller
    {

        [Route("shop")]
        public IActionResult Index()
        {
            return View();
        }

    }

}