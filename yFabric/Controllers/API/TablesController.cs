using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Repository.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Repository.Helpers;
using yFabric.DataContexts;
using HttpPost = System.Web.Mvc.HttpPostAttribute;
using HttpGet = System.Web.Mvc.HttpGetAttribute;

using System.Threading;
using Kendo.Mvc.UI;
using Newtonsoft.Json.Linq;

namespace yFabric.Controllers.API
{
    public class TablesController : ApiController
	{
		//[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
		public IEnumerable<dynamic> GetRestaurants(int take)
		{
			List<dynamic> list = StaticCache.GetRestaurants().Take(take).ToList();
			//throw new ApplicationException("test");
			//var myJson = bsonDocs.ToJson();
			//var response = this.Request.CreateResponse(HttpStatusCode.OK);
			//response.Content = new StringContent(myJson, Encoding.UTF8, "application/json");
			return list;

		}
		[HttpPost]
		public async Task<IHttpActionResult> UpdateRestaurant(dynamic models)
		{
			var obj = await Request.Content.ReadAsFormDataAsync();
			//var x = await Request.Content.ReadAsHttpRequestMessageAsync();
			var y = await Request.Content.ReadAsStringAsync();
			//todo
			// Get a list of products from a database.
			List<dynamic> list = new List<dynamic>();

			// Write the list to the response body.
			//HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, list);
			return new TextResult("{}", Request);
		}
		private static async Task AddDocument(IMongoCollection<BsonDocument> bsonDocs, List<dynamic> list)
		{
			var filter = new BsonDocument();
			var count = 0;
			using (var cursor = await bsonDocs.FindAsync(filter))
			{
				while (await cursor.MoveNextAsync())
				{
					var batch = cursor.Current;
					foreach (var document in batch)
					{
						// process document
						list.Add(document);
						count++;
					}
				}
			}
		}

		public object List { get; set; }

	}
}
