using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vanderkorn.ER.Startup))]
namespace Vanderkorn.ER
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureSignalR(app);
        }
    }
}
