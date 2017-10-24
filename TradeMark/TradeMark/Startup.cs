using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TradeMark.Startup))]
namespace TradeMark
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
