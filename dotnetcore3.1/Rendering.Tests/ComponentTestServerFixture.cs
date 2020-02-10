using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Rendering.Tests
{
    public class ComponentTestServerFixture : WebApplicationFactory<ComponentTestStartup>
    {
        public TService GetRequiredService<TService>()
        {
            if (Server == null)
            {
                CreateDefaultClient();
            }

            return Server.Host.Services.GetRequiredService<TService>();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var hostBuilder = new WebHostBuilder();
            // Solution fixing the problem:
            // https://github.com/dotnet/aspnetcore/issues/17655#issuecomment-581418168
            hostBuilder.ConfigureAppConfiguration((context, b) =>
            {
                context.HostingEnvironment.ApplicationName = typeof(ViewRender).Assembly.GetName().Name;
            });
            return hostBuilder.UseStartup<ComponentTestStartup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSolutionRelativeContentRoot("Rendering");
        }
    }
}
