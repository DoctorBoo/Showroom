using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace yFabric.Controllers.API
{
    public class RestaurantsController : ApiController
    {
        // GET: api/Restaurants
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Restaurants/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Restaurants
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Restaurants/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Restaurants/5
        public void Delete(int id)
        {
        }
    }
}
