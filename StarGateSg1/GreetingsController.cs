using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace StarGateSg1
{
    public class GreetingsController: ApiController
    {
        public Greetings Get ()
        {
            return new Greetings { Text = "Hello World!" };
        }
    }
}
