using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Extensions;
using dotNETCoreRESTAPI.Interfaces;

namespace dotNETCoreRESTAPI.Controllers
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();
            
            return services;
        }
    }
}