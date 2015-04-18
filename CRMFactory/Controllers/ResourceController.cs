using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CRMFactory.Controllers
{
    public class ResourceController : ApiController
    {
        // GET: api/Resource
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Resource/5
		public string Get(string resourceKey)
        {
            return "value";
        }

        // POST: api/Resource
        public void Post([FromBody]string value)
        {
        }


    }
}
