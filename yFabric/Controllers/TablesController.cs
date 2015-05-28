using Kendo.Mvc.UI;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yFabric.Models;
using Kendo.Mvc.Extensions;

namespace yFabric.Controllers
{
    public class TablesController : Controller
    {
        // GET: Tables
		public ActionResult Index([DataSourceRequest]DataSourceRequest request)
		{
			return View();
		}
		public ActionResult Customers_Read([DataSourceRequest]DataSourceRequest request)
		{

			return Json(GetCustomers().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
		}

		private static IEnumerable<User> GetCustomers()
		{
			var usersContext = new SygnionDb();
			var appUsers = usersContext.Users.ToList<User>();
			
			return appUsers;
		}
    }
}