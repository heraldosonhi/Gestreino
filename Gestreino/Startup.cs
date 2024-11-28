using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gestreino.Startup))]
namespace Gestreino
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
