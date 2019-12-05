using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rendering.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureRenderingServices(this IServiceCollection services)
        {
            services.AddTransient<ViewRender, ViewRender>();
            services.AddTransient<IRenderingApi, RenderingApi>();

            return services;
        }
    }
}
