using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Koleg.io.Startup))]
namespace Koleg.io
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
