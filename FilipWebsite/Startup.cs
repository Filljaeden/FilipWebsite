using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FilipWebsite.Startup))]
namespace FilipWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
