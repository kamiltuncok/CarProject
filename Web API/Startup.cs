using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Sırları git'e girmeyen appsettings.Local.json'dan al ve DbContext'in okuyabilmesi için
            // bağlantı dizesini process ortam değişkenine köprüle (parametresiz DbContext new'lendiği için).
            var connectionString = Configuration.GetConnectionString("RentACar");
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                Environment.SetEnvironmentVariable("RENTACAR_CONNECTION_STRING", connectionString);
            }

            services.AddControllersWithViews();

            services.AddCors();


            services.AddHangfire(config => config.UseMemoryStorage());
            services.AddHangfireServer();
            services.AddHttpClient<IPricingService, PricingManager>();

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            if (tokenOptions == null || string.IsNullOrWhiteSpace(tokenOptions.SecurityKey))
            {
                throw new InvalidOperationException(
                    "JWT SecurityKey yapılandırılmamış. 'Web API/appsettings.Local.json' içinde TokenOptions:SecurityKey " +
                    "tanımlayın ya da TokenOptions__SecurityKey ortam değişkenini ayarlayın.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    };
                });

            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule()
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureCustomExceptionMiddleware();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new Web_API.Security.HangfireDashboardAuthorizationFilter() }
            });

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            RecurringJob.AddOrUpdate<IPricingService>(
        "update-prices-job",
        service => service.UpdateAllPricesAsync(),
        "*/1 * * * *");
        }


    }
}
