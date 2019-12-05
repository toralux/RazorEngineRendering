using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rendering.Tests
{
    public class ComponentTestStartup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
        }
    } 
}
