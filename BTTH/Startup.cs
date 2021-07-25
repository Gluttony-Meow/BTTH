using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTTH.Startup))]
namespace BTTH
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
