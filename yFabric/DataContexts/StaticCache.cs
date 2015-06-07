using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using yFabric.Controllers.API;
using Repository.Helpers;
using Repository.Contexts;

namespace yFabric.DataContexts
{
	[DataObject]
	public class StaticCache
	{
		static string _cacheId = "cached-restaurants";
		public async static void LoadStaticCache()
		{
			IMongoCollection<BsonDocument> bsonDocs = DoGetRestaurants();
			List<dynamic> restaurants = new List<dynamic>();
			try
			{
				await restaurants.AddBsonDocumentsAsync(bsonDocs);
			}
			catch (Exception)
			{
				throw;
			}

			HttpRuntime.Cache.Insert(
				/* key */                _cacheId,
				/* value */              restaurants,
				/* dependencies */       null,
				/* absoluteExpiration */ Cache.NoAbsoluteExpiration,
				/* slidingExpiration */  Cache.NoSlidingExpiration,
				/* priority */           CacheItemPriority.NotRemovable,
				/* onRemoveCallback */   null);
		}
		private static IMongoCollection<BsonDocument> DoGetRestaurants()
		{
			string document = "restaurants";
			string test = "test";
			var mongoContext = new MongoCtx();
			var collection = mongoContext.GetData(test, document);
			return collection;
		}

		[DataObjectMethodAttribute(DataObjectMethodType.Select, true)]
		public static IEnumerable<dynamic> GetRestaurants()
		{
			return HttpRuntime.Cache[_cacheId] as IEnumerable<dynamic>;
		}
	}
}