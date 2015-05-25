using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yFabric.Counters
{
    public class PerfCounterService
    {
        List<PerCounterWrapper> _counters = new List<PerCounterWrapper>();

        public PerfCounterService()
        {
            _counters.Add(new PerCounterWrapper("Processor", "Processor", "% Processor Time", "_Total"));
            try
            {
                //_counters.Add(new PerCounterWrapper("Paging", "Memory", "Pages/Second"));
                //_counters.Add(new PerCounterWrapper("Disk", "Physical Disk", "% Disk Time", "_Total"));
            }
            finally
            {

            }
        }

        public dynamic GetResults()
        {
            return _counters.ToList<PerCounterWrapper>().Select(c => new { name = c.Name, value = c.Value });
        }
    }
}
