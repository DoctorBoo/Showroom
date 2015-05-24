using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace yFabric.Counters
{
    public class PerCounterWrapper
    {
        public PerCounterWrapper(string name , string category, string counter , string instance = "")
        {
            _counter = new PerformanceCounter(category, counter, instance, readOnly: true);
        }

        public int Name { get; set; }
        public float Value { get
            {
                return _counter.NextValue();
            }
        }

        PerformanceCounter _counter;

    }
}