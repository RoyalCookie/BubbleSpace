using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bubblespace.Startup))]
namespace Bubblespace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
