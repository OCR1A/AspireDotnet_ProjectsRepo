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

        //Register account controllers
        [Route("/register-hub")]
        public IActionResult RegisterHub()
        {
            return View();
        }

        [Route("/register-email")]
        public IActionResult RegisterEmail()
        {
            return View();
        }

        [Route("/register-name")]
        public IActionResult RegisterName()
        {
            return Content("<h2>Register name:</h2>", "text/html");
        }

        [Route("/register-cellphone")]
        public IActionResult RegisterCellphone()
        {
            return Content("<h2>Register Cellphone:</h2>", "text/html");
        }

        [Route("/register-password")]
        public IActionResult RegisterPassword()
        {
            return Content("<h2>Register Password:</h2>", "text/html");
        }

    }

}