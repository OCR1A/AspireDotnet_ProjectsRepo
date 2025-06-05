using Microsoft.AspNetCore.Mvc;
namespace LegalNamespace.Controllers
{

    public class LegalController : Controller
    {

        [Route("/help/terms_of_service")]
        public IActionResult Terms()
        {
            return View();
        }

        [Route("/help/privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

    }

}