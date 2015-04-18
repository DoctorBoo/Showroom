using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRMFactory.Startup))]
namespace CRMFactory
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
