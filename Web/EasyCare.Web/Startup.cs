using AutoMapper;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Authentication;
using EasyCare.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data.Entity.Core;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
namespace EasyCare.Web
{
    public class Startup
    {
        public static string ScopeRead;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EasyDatabaseConnection")));
            services.AddRepositories();
            services.AddAutoMapper(typeof(Startup));
            services.AddServices();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                      .AddJwtBearer(jwtOptions =>
                      {
                          jwtOptions.Authority = $"https://{Configuration["AzureAdB2C:AzureADB2CHostname"]}/{Configuration["AzureAdB2C:Tenant"]}/{Configuration["AzureAdB2C:Policy"]}/v2.0/";
                          jwtOptions.Audience = Configuration["AzureAdB2C:ClientId"];
                          jwtOptions.Events = new JwtBearerEvents
                          {
                              OnAuthenticationFailed = context => { return Task.FromResult(context.Exception.Message); }
                          };
                      });
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddOptions<NotificationHubOptions>()
                .Configure(Configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EasyDatabaseConnection")));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerWithUI();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            //ScopeRead = Configuration["AzureAdB2C:ScopeRead"];
            app.UseAuthentication();
            app.UseMvc();
            //app.UseAuthorization();
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}