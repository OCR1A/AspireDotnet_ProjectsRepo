using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using IdentityManager.Models;
using IdentityManager.Models.ViewModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity.UI.Services;
using IdentityManager.Services;


namespace IdentityManager.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [Route("/sign-in")]
        public IActionResult SignIn(string returnurl = null)
        {
            //await _signInManager.SignOutAsync();
            if (!_signInManager.IsSignedIn(User))
            {
                ViewData["ReturnUrl"] = returnurl;
                return View();
            }
            else
            {
                return RedirectToAction("AccountSite", "Account");
            }
        }

        [HttpPost("/sign-in")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnurl = null)
        {

            //Null coalescing to redirect to index if returnur is null
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/home");

            if (ModelState.IsValid && _signInManager.IsSignedIn(User) == false)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                    return View(model);
                }

            }

            return View(model);
        }

        [Route("/full-register")]
        public IActionResult FullRegister(string returnurl = null)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                ViewData["ReturnUrl"] = returnurl;
                RegisterViewModel registerViewModel = new();
                return View(registerViewModel);
            }
            else
            {
                return RedirectToAction("AccountSite", "Account");
            }
        }

        [HttpPost("/full-register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FullRegister(RegisterViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/index");
            if (ModelState.IsValid && _signInManager.IsSignedIn(User) == false)
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
                    return LocalRedirect(returnurl);    
                }
                else
                {
                    AddErrors(result);
                }
            }
            return View(model);
        }

        [HttpPost("/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("/account")]
        public IActionResult AccountSite()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }

        [Route("/password-recovery")]
        public IActionResult ForgotPassword()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction("AccountSite", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new
                {
                    userid = user.Id,
                    code
                }, protocol: HttpContext.Request.Scheme);
                _emailSender.SendEmailAsync(model.Email, "Reset Password - Identity Manager",
                $"Please reset your password by clicking here: <a hreg='{callbackurl}'>link</a>");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
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