using Microsoft.AspNetCore.Mvc;
using ApplicationUserNamespace.Model;

namespace AccountNamespace.Controllers
{

    public class AccountController : Controller
    {

        [Route("/sign-in")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/sign-in")]
        public IActionResult Login(ApplicationUser   myPerson)
        {

            if (!ModelState.IsValid)
            {

                foreach (var value in ModelState.Values)
                {

                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }

                }

                Console.WriteLine();
                return BadRequest();

            }

            Console.WriteLine($"User: {myPerson.Email} logged in successfully!");
            return Json(myPerson);

        }

    }

}