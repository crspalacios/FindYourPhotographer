using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindYourPhotographer.Backend.Startup))]
namespace FindYourPhotographer.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
