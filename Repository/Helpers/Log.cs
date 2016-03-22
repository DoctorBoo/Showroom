using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public static class Log<TLogger>
    {
        public static ILog Write = LogManager.GetLogger(typeof(TLogger));
    }
}
