using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using yFabric.Counters;

namespace yFabric.Hubs
{
    public class PerfHub: Hub
    {
        public PerfHub()
        {
            StartCounterCollection();
        }

        public  void StartCounterCollection()
        {
			//var task = Task.Factory.StartNew(async () =>
			//{
			//	var perfService = new PerfCounterService();
			//	while (true)
			//	{
			//		var results = perfService.GetResults();
			//		Clients.All.newCounters(results);
			//		await Task.Delay(2000);
			//	};
			//}, TaskCreationOptions.LongRunning);
        }
    }
}