using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Testv3.Startup))]
namespace Testv3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
