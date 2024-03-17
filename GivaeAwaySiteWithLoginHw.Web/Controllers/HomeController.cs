using GivaeAwaySiteWithLoginHw.Web.Models;
using GiveAwaySiteWithLoginHw.Web.Models;
using GiveAwayWithLoginHw.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GiveAwaySiteWithLoginHw.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connection = @"Data Source=.\sqlexpress; Initial Catalog=Giveaway; Integrated Security=true;";

        public IActionResult Index()
        {
            Repisotory repo = new(_connection);
            var currentUser = User.Identity;

            var vm = new IndexViewModel { Ads = repo.GetAllAds() };
            if (currentUser.IsAuthenticated)
            {
                vm.CurrentUserId = repo.GetUserByEmail(currentUser.Name).Id;
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            Repisotory repo = new(_connection);
            repo.DeleteAd(id);
            return Redirect("/home/index");
        }

        public IActionResult NewAd()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return Redirect("/Account/Login");
        }

        [HttpPost]
        public IActionResult NewAd(string text)
        {
            Repisotory repo = new(_connection);
            repo.NewAd(text, repo.GetUserByEmail(User.Identity.Name).Id);
            return Redirect("/home/index");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            Repisotory repo = new(_connection);
            var vm = new MyAccountViewModel {Ads=repo.GetAdsByUserId(repo.GetUserByEmail(User.Identity.Name).Id) };
            return View();
        }
    }
}