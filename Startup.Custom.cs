using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Doleance.Data;
using Doleance.Models;
using Doleance.Authentication;
using Radzen;
using System.Globalization;
using Microsoft.AspNetCore.HttpOverrides;
using Doleance.Services;

namespace Doleance
{
    public partial class Startup
    {
        partial void OnConfigureServices(IServiceCollection services)
        {
              
            var cultureInfo = new CultureInfo("fr-FR");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.Configure<RequestLocalizationOptions>(options =>
            {
                
                options.SupportedCultures[0].NumberFormat.NumberDecimalSeparator=".";
                
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });


            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<MailService>();
            services.AddControllers();
        }

        partial void OnConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
        }

        public static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Constants.admin))
                await roleManager.CreateAsync(new IdentityRole(Constants.admin));

            if (!await roleManager.RoleExistsAsync(Constants.structadmin))
                await roleManager.CreateAsync(new IdentityRole(Constants.structadmin));

            if (!await roleManager.RoleExistsAsync(Constants.user))
                await roleManager.CreateAsync(new IdentityRole(Constants.user));

        }
    }
    }