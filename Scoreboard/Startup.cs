using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Scoreboard.Startup))]
namespace Scoreboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
