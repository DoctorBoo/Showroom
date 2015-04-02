using Repository.Contexts;
using Repository.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Shogun.Contexts
{
	public static class BookStore
	{
		public static void LoadStore(string serverfolderPath, string fileName)
		{
			string appDataPath = serverfolderPath; //Server.MapPath("~/app_data");
			string xmlPath = Path.Combine(appDataPath, fileName);

			XDocument catalog = XDocument.Load(xmlPath);

			var books = from b in catalog.Elements("catalog").Elements("book")
						select b;

			var query = from b in catalog.Elements("catalog").Elements("book")
						select new Book 
						{ 
							Id = b.Attribute("id").Value, 
							Author = b.Elements("author").FirstOrDefault().Value ,
							Category = 0,//int.Pars b.Elements("genre").FirstOrDefault().Value,
							Title = b.Elements("title").FirstOrDefault().Value,
							Price = decimal.Parse( b.Elements("price").FirstOrDefault().Value)
						};
			
			BookStoreCtx.Save(query.ToList());
		}
		

	}
}