using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace StarGateSg1
{
    using System.IO;
    using System.Web.Http;
    using AppFunc = Func<IDictionary<string, object>, Task>;
    class Program
    {
        static string _port = "8082";

        static void Main(string[] args)
        {
            string uri = string.Format("http://localhost:{0}", _port);
            using (WebApp.Start<StartUp>(uri))
            {
                Console.WriteLine(string.Format("Server started listening {0}", _port));
                Console.ReadKey();
                Console.WriteLine("Stopping.");
            }
        }
    }

    public class StartUp
    {
        public void Configuration (IAppBuilder app)
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

            app.Use(async (env, next) => {

                Console.WriteLine("Requesting: {0}", env.Request.Path);
                await next();
                Console.WriteLine("Responding: {0}", env.Response.StatusCode);
            });
            ConfigureWebApi(app);
            app.UseHelloWorld();

            //app.UseWelcomePage();
            //app.Run( ctx => {
            //    return ctx.Response.WriteAsync("Hello World!");
            //});
        }

        private void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            app.UseWebApi(config);
        }
    }

    public static class AppbuilderExtensions
    {
        public static void UseHelloWorld(this IAppBuilder app)
        {
            app.Use<HelloComponent>();
        }

    }
    public class HelloComponent
    {
        AppFunc _next;
        public HelloComponent(AppFunc next)
        {
            _next = next;
        }
        public Task Invoke(IDictionary<string,object> environment)
        {
            //await _next(environment); // inspect environment           
            var response = environment["owin.ResponseBody"] as Stream;
            using (var writer = new StreamWriter(response))
            {
                return writer.WriteAsync("Hello World!");
            }
        }
    }
}
