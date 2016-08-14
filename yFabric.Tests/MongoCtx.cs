using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
	public class MongoCtx
	{
		protected static IMongoClient _client;
		protected static IMongoDatabase _database;

		public IMongoCollection<BsonDocument> GetData(string catalogDb, string document)
		{
			_client = new MongoClient();
			_database = _client.GetDatabase(catalogDb);
			var collection = _database.GetCollection<BsonDocument>(document);
			//await collection.InsertOneAsync(document);

			return collection;
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
