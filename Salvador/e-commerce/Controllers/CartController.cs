using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartNamespace.Controllers
{

    public class CartController : Controller
    {

        [Route("/cart")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

    }

}