using ClientMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace StarGateSg1
{
    public class GreetingsController: ApiController
    {
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpGet, ActionName("hello")]

        public IHttpActionResult Get()
        {
            DateTime datetime = DateTime.Now;

            return Ok("Send on " + datetime.ToShortTimeString());
            //return new Greetings { Text = String.Format("{0} : Hello World!!!!!!!", datetime.ToShortTimeString()) };
        }

        [Route("send-email")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, ActionName("send-email")]
        public IHttpActionResult SendMail()
        {
            using (OfficeFactory factory = new OfficeFactory("smtp.gmail.com", 587, "sygnion", "sygnion01!"))
            {
                DateTime datetime = DateTime.Now;
                return Ok(String.Format("Sent on {0} {1}", datetime.ToShortDateString(), datetime.ToShortTimeString()));
            }
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, ActionName("sendmail2")]
        public IHttpActionResult SendMail2()
        {
            using (OfficeFactory factory = new OfficeFactory("smtp.gmail.com", 587, "sygnion", "sygnion01!"))
            {
                DateTime datetime = DateTime.Now;
                return Ok(String.Format("Sent on {0} {1}", datetime.ToShortDateString(), datetime.ToShortTimeString()));
            }
        }
    }
}
