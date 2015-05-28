using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using yFabric.Models;

namespace yFabric.Controllers
{
	[RequireHttps]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			ViewBag.Name = User.GetNick();
			object model = ViewBag.Name;

			return View(model);
        }
    }
}
