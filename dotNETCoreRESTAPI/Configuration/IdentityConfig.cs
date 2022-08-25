using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dotNETCoreRESTAPI.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var builder = WebApplication.CreateBuilder();
            var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlite(connectionString));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }       
    }
}