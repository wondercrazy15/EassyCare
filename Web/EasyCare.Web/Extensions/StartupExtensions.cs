using System;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.File;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCare.Web.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddTransient<ISeniorRepository, SeniorRepository>();
            services.AddTransient<ISingedUpUserRepository, SignedUpUserRepository>();
            services.AddTransient<ISupervisorRepository, SupervisorRepository>();
            services.AddTransient<ISensorMessageRepository, SensorMessageRepository>();
            services.AddTransient<ITANRepository, TANRepository>();
            services.AddTransient<ICalendarEventRepository, CalendarEventRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<INotificationMessageRepository, NotificationMessageRepository>();
            services.AddTransient<IDrugsRepository, DrugsRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IParticipantRepository, ParticipantRepository>();
            services.AddTransient<ICalendarSupervisorsRepository, CalendarSupervisorsRepository>();
            services.AddTransient<ICalendarEventSchedulersRepository, CalendarEventSchedulersRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<INotificationService, NotificationHubService>();
            services.AddSingleton<IFileService, FileService>();
            
            return services;
        }
        
        public static AuthenticationBuilder AddApiKeyAuth(this AuthenticationBuilder builder, 
            Action<ApiKeyAuthOptions> options)
        {
            return builder.AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(
                    ApiKeyAuthOptions.DefaultScheme,
                    options);
        }

        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            return app;
        }
    }
}