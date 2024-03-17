using GiveAwayWithLoginHw.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GiveAwaySiteWithLoginHw.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connection = @"Data Source=.\sqlexpress; Initial Catalog=Giveaway; Integrated Security=true;";

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = (string)TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Repisotory repo = new(_connection);
            if(!repo.Authenticated(email, password))
            {
                TempData["Message"] = "Invalid Login!";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                   new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "roles"))
               ).Wait();

            return Redirect("/home/index");
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {
            Repisotory repo = new(_connection);
            repo.CreateAccount(user);

            return Redirect("/Account/Login");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();

            return Redirect("/account/login");
        }
    }
}
