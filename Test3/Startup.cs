using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Test3.Startup))]
namespace Test3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
