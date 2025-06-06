using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IdentityManager.Models;
using IdentityManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IdentityManager.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("/full-register")]
        public IActionResult FullRegister()
        {
            RegisterViewModel registerViewModel = new();
            return View(registerViewModel);
        }

        [Route("/sign-in")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("/sign-in")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Console.WriteLine($"{model.Email} logged in successfully!");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                    Console.WriteLine("Error to log in!");
                    return View(model);
                }

            }

            return View(model);
        }

        [HttpPost("/full-register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FullRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOFF()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                    return View(model);
                }

            }

            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

        }

    }

}
/*using Microsoft.AspNetCore.Mvc;
using IdentityManager.Models;
using IdentityManager.Models.ViewModels;

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

}*/