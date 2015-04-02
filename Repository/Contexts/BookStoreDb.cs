using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
	public class BookStoreDb: DbContext
	{
		public BookStoreDb()
			: base("BookStore")
		{

		}
		public DbSet<Book> Books { get; set; }
	}
}
