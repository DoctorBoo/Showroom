using Kendo.Mvc.UI;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yFabric.Models;
using Kendo.Mvc.Extensions;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Repository.Helpers;
using System.Net;

namespace yFabric.Controllers
{
	[Authorize]
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
		public ActionResult Restaurants_Read([DataSourceRequest]DataSourceRequest request)
		{
			return Json(GetRestaurants().ToJson());
		}
		public async Task<ActionResult> Restaurants_Update([DataSourceRequest]DataSourceRequest request)
		{
			return View("{}");  
		}
		public ActionResult GetRestaurants()
		{
			var myJson = DoGetRestaurants().ToJson();					
			return View(myJson);
		}

		private static IMongoCollection<BsonDocument> DoGetRestaurants()
		{
			string document = "restaurants";
			string test = "test";
			var mongoContext = new MongoCtx();
			var collection = mongoContext.GetData(test, document);
			return collection;
		}
    }
}