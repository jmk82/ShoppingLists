using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ostoslista.Startup))]
namespace Ostoslista
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
