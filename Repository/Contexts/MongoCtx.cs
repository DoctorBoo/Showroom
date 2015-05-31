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
			var connectionString = "mongodb://localhost:27017?connect=automatic";

			//take database name from connection string
			var _databaseName = MongoUrl.Create(connectionString).DatabaseName;
			_client = new MongoClient(connectionString);			
			//var _server = _client.GetServer() as MongoClient;
						
			_database = _client.GetDatabase(catalogDb);
			var collection = _database.GetCollection<BsonDocument>(document);
			//await collection.InsertOneAsync(document);

			return collection;
		}
	}
}
