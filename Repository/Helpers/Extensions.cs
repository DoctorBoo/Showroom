using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;

namespace Repository.Helpers
{
	public static class Extensions
	{
		public static async Task<IList> AddBsonDocumentsAsync(this IList list, IMongoCollection<BsonDocument> bsonDocs)
		{
			var filter = new BsonDocument();

			try
			{
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
			catch (Exception)
			{				
				throw;
			}
			return list;
		}

		/// <summary>
		/// Counts async the items in collection.
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static long Count(this IMongoCollection<BsonDocument> list)
		{
			return DoCount(list);
		}
		public static long DoCount(IMongoCollection<BsonDocument> list)
		{
			var result = GetDocumentCount(list, "{}");
			result.Wait();

			long count = result.Result;
			return count;
		}
		private async static Task<long> GetDocumentCount(IMongoCollection<BsonDocument> list, FilterDefinition<BsonDocument> filter)
		{
			long count = await list.CountAsync(filter);
			return count;
		}
	}
}
