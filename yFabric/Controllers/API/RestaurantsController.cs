using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using Repository.Entities;
using Microsoft.Data.OData;
using yFabric.DataContexts;

namespace yFabric.Controllers.API
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Repository.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Restaurant>("Restaurants");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RestaurantsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/Restaurants
        [EnableQuery]
        public async Task<IHttpActionResult> GetRestaurants(ODataQueryOptions<Restaurant> queryOptions)
        {
            // validate the query.
            try
            {
                List<Restaurant> list = new List<Restaurant>{
			     new Restaurant(){id="12",Name="Sjon",Time=new DateTime(), cuisine="Surinaams", Graded=new List<string>{"A","A"}},
                  new Restaurant(){id="14",Name="Piet",Time=new DateTime(), cuisine="Snacks", Graded=new List<string>{"A","A"}}
			    };
                var dynamics = StaticCache.GetRestaurants().ToList();

                return Ok<IEnumerable<Restaurant>>(list);
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

           
            //return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/Restaurants(5)
        public async Task<IHttpActionResult> GetRestaurant([FromODataUri] string key, ODataQueryOptions<Restaurant> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<Restaurant>(restaurant);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Restaurants(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Restaurant> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(restaurant);

            // TODO: Save the patched entity.

            // return Updated(restaurant);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Restaurants
        public async Task<IHttpActionResult> Post(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(restaurant);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Restaurants(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Restaurant> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(restaurant);

            // TODO: Save the patched entity.

            // return Updated(restaurant);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Restaurants(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
