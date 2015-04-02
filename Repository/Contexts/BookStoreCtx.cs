using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contexts
{
	public class BookStoreCtx: DbContext
	{
		public BookStoreCtx()
			: base("BookStore")
		{

		}
		public DbSet<Book> Books { get; set; }

		public static void Save(IList<Book> books)
		{
			using (var ctx = new BookStoreCtx())
			{
				foreach (var book in books)
				{
					if (ctx.Books.Any( b => b.Id.Equals(book.Id)))
					{
						ctx.Entry(book).State = EntityState.Modified;
					}
					else
					{
						ctx.Entry(book).State = EntityState.Added;
					}
					ctx.SaveChanges();
				}
				
			}
		}
	}
}
