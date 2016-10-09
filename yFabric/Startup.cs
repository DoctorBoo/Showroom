using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(yFabric.Startup))]

namespace yFabric
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			#region dump environment
            //app.Use(async (env, next) =>
            //{

            //    foreach (var item in env.Environment)
            //    {
            //        Console.WriteLine("{0}:{1}", item.Key, item.Value);
            //    }
            //    await next();
            //});
			#endregion
            ConfigureAuth(app);

            // Any connection or hub wire up and configuration should go here
            //app.MapSignalR();
            #region Signalr
            var hubConfiguration = new HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            hubConfiguration.EnableJavaScriptProxies = true;
            app.MapSignalR("/signalr", hubConfiguration);
            #endregion
        }
    }
}
