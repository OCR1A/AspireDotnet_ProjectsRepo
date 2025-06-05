using Microsoft.AspNetCore.Mvc;
using IdentityManager.Models;

namespace IdentityManager.Controllers
{

    public class AccountController : Controller
    {

        [Route("/sign-in")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("/sign-in")]
        public IActionResult SignIn(ApplicationUser myPerson)
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

        [Route("/register")]
        public IActionResult RegisterEmail()
        {
            return View();
        }

        [Route("/register-hub")]
        public IActionResult RegisterHub()
        {
            return View();
        }

        [Route("/mail/registered")]
        public IActionResult EmailRegister()
        {
            return Content("Email registered successfully!", "text/html");
        }

    }

}