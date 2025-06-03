/*Controllers for all things related to user handling*/

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using ApplicationUserNamespace.Model;

namespace PersonNamespace.Controllers
{

    public class PersonController : Controller
    {

        [Route("/register")]
        [Route("/register/{newPerson.Name}/{newPerson.Email}/{newPerson.Password}/{newPerson.ConfirmPassword}/{newPerson.Price}")]
        public IActionResult Register([FromRoute] ApplicationUser newPerson)
        {

            if (!ModelState.IsValid)
            {

                //Print errors
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

            Console.WriteLine($"Person {newPerson.Name} created succesfully!");
            return Json(newPerson);

        }

    }

}