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
using System.IO;
using Repository.Entities;
using System.Web.Script.Serialization;

namespace yFabric.Controllers.API
{
    public class TablesController : ApiController
	{
		//[OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
		public IEnumerable<dynamic> GetRestaurants(int take)
		{
			List<Restaurant> list = new List<Restaurant>{
			 new Restaurant(){id="12",Name="Sjon",Time=new DateTime(), cuisine="Surinaams", Graded=new List<string>{"A","A"}},
             new Restaurant(){id="34",Name="Piet",Time=new DateTime(), cuisine="Vlaams", Graded=new List<string>{"A","A"}}
			};
            var dynamics = StaticCache.GetRestaurants().Take(take).ToList();
			//throw new ApplicationException("test");
			//var myJson = bsonDocs.ToJson();
			//var response = this.Request.CreateResponse(HttpStatusCode.OK);
			//response.Content = new StringContent(myJson, Encoding.UTF8, "application/json");
            return list;

		}
		[HttpPost]
        public async Task<IHttpActionResult> UpdateRestaurant([DataSourceRequest]DataSourceRequest request, int? id)
		{
			var obj = Request.GetOwinContext();

			var inputStream = obj.Environment["owin.RequestBody"] as Stream;
			byte[] result;

			using (Stream SourceStream = inputStream)
			{
				result = new byte[SourceStream.Length];
				await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
			}
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            var stringified = Encoding.Default.GetString(result);
            //For inline editing
            Dictionary<string, object> jsonfiedPair = json_serializer.DeserializeObject(stringified) as Dictionary<string, object>;
            //For incell editing
            object[] objects = json_serializer.DeserializeObject(stringified) as object[];

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
