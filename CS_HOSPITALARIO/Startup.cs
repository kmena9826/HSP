using Microsoft.Owin;
using Owin;
using CS_HOSPITALARIO.Models;

[assembly: OwinStartupAttribute(typeof(CS_HOSPITALARIO.Startup))]
namespace CS_HOSPITALARIO
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }       
    }
}