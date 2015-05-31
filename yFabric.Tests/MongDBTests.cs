using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Contexts;
using Repository.Helpers;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using System.Globalization;

namespace yFabric.Tests
{
	[TestClass]
	public class MongoDBTests
	{
		private FilterDefinition<BsonDocument> filter ;
		[TestMethod]
		public void GetDocuments()
		{
			var mongoDb = new MongoCtx();
			filter = "{}";
			document = "restaurants";
			test = "test";
			IMongoCollection<BsonDocument> list = mongoDb.GetData(test, document);

			long count = list.Count();
			Assert.AreEqual(25359, count);
		}
		[TestMethod]
		public void GetDocuments2()
		{
			var mongoDb = new MongoCtx();
			filter = "{}";
			document = "restaurants";
			test = "test";

			IMongoCollection<BsonDocument> bsonDocs = mongoDb.GetData(test, document);
			List<dynamic> list = new List<dynamic>();
			var waitResult = list.AddBsonDocumentsAsync(bsonDocs);
			Task.WaitAll(waitResult);

			var result = waitResult.Result;
			long count = list.Count;
			Assert.AreEqual(25359, count);
		}
		[TestMethod]
		public void CulturesTest()
		{
			List<string> list = new List<string>();
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
			{
				string specName = "(none)";
				try 
				{ 
					specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; 
				}
				catch { }
				list.Add(String.Format("{0,-12}{1,-12}{2} {3,-1}", ci.Name, specName, ci.EnglishName));
			}

			list.Sort();  // sort by name

			// write to console
			Console.WriteLine("CULTURE   SPEC.CULTURE  ENGLISH NAME");
			Console.WriteLine("--------------------------------------------------------------");
			foreach (string str in list)
				Console.WriteLine(str);
		}

		public string document { get; set; }

		public string test { get; set; }
	}
}
