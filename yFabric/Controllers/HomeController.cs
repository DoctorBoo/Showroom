using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using yFabric.Models;

namespace yFabric.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			var usersContext = new ApplicationDbContext();
			var appUser = (from u in usersContext.Users
							where u.UserName.Equals(User.Identity.Name)
							select new { Name = u.Nick!= null && u.Nick.Trim() != "" ? u.Nick : User.Identity.Name}).FirstOrDefault();
			ViewBag.Name = appUser.Name;
            return View();
        }
    }
}
