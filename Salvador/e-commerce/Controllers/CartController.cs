using Microsoft.AspNetCore.Mvc;

namespace CartNamespace.AddControllersWithViews
{

    public class CartController : Controller
    {

        [Route("/cart")]
        public IActionResult Index()
        {
            return View();
        }

    }

}