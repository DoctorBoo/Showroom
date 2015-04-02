using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
	public class Book
	{
		[Required]
		public string Id { get; set; }
		[Required]
		[StringLength(255)]
		public string Author { get; set; }
		[Required]
		[StringLength(255)]
		public string Title { get; set; }
		[Required]
		public Genre Category{ get; set; }
		public DateTime? PublishDate { get; set; }
		[Required]
		public decimal Price { get; set; }
		public string Description { get; set; }
	}
}
