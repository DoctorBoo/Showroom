using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace yFabric.Controllers
{
	public class TextResult : IHttpActionResult
	{
		string _value;
		HttpRequestMessage _request;
		private string p;
		private HttpRequestBase Request;

		public TextResult(string value, HttpRequestMessage request)
		{
			_value = value;
			_request = request;
		}

		public TextResult(string p, HttpRequestBase Request)
		{
			// TODO: Complete member initialization
			this.p = p;
			this.Request = Request;
		}
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage()
			{
				Content = new StringContent(_value),
				RequestMessage = _request
			};
			return Task.FromResult(response);
		}
	}
}