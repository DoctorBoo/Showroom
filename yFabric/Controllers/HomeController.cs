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
			ViewBag.Name = User.GetNick();

			return View(User.GetNick() as object);
        }
    }
}
