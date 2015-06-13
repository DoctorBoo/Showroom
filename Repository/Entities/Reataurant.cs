using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
	public class Restaurant
	{
		public string id { get; set; }
		public string Name { get; set; }

		public DateTime Time { get; set; }
		public string cuisine { get; set; }
		public string location { get; set; }
		public List<string> Graded { get; set; }
	}
}
