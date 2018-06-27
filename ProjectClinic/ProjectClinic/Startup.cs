using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectClinic.Startup))]
namespace ProjectClinic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
