using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Etherean.Startup))]
namespace Etherean
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
