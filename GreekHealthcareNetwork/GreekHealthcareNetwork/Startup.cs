using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GreekHealthcareNetwork.Startup))]
namespace GreekHealthcareNetwork
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
