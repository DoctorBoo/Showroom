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
    using System.Web.Http.SelfHost;
    using System.Net;
    class Program
    {
        const string _port = "44322";
        public static readonly string uri = string.Format("https://127.0.0.1:{0}", _port); 
        static void Main(string[] args)
        {
            List<Exception> q = new List<Exception>();
            //q = StartSslWebApi();

            try
            {
                var options = new StartOptions(url: uri)
                {
                    ServerFactory = "Microsoft.Owin.Host.HttpListener",         
                    AppStartup = "StartUp",
                    Port= int.Parse(_port)
                };
                using (WebApp.Start <StartUp>(options)) //<StartUp>
                {
                    Console.WriteLine(string.Format("Server started listening {0}", _port));
                    Console.ReadKey();
                    Console.WriteLine("Stopping.");
                }
            }
            catch (AggregateException ex)
            {
                q.Add(ex);
            }
            catch (Exception ex)
            {
                q.Add(ex);
            }
        }
        /// <summary>
        /// Old Web API with .NET 4.0 support
        /// </summary>
        /// <returns></returns>
        private static List<Exception> StartSslWebApi()
        {
            string sslAddress = "https://127.0.0.1:44322";
            var config = new HttpSelfHostWindowsAuthenticationConfiguration(sslAddress);
            List<Exception> q = new List<Exception>();

            //Need tp refactor to use the route in Startup.cs
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                try
                {
                    server.OpenAsync().Wait();
                }
                catch (AggregateException ex)
                {
                    q.Add(ex);
                }
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
            return q;
        }
    }

    public class StartUp
    {
        public void Configuration (IAppBuilder app)
        {            
            #region dump environment
            app.Use(async (env, next) =>
            {

                foreach (var item in env.Environment)
                {
                    Console.WriteLine("{0}:{1}", item.Key, item.Value);
                }
                await next();
            });
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
            //old: var config = new HttpConfiguration();

            //HttpListener listener = (HttpListener)app.Properties["System.Net.HttpListener"];
            //listener.AuthenticationSchemes = AuthenticationSchemes.IntegratedWindowsAuthentication;

            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
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
