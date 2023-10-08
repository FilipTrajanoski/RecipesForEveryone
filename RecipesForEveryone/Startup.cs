using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecipesForEveryone.Startup))]
namespace RecipesForEveryone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
