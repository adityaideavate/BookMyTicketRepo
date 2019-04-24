using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookMyTicket.Startup))]
namespace BookMyTicket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
