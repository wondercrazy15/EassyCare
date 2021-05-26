using Autofac;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Services;

namespace EasyCare.DI
{
    public class AppSetup 
    {
        public IContainer CreateContainer() 
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            var clientFactory = ClientFactory.Create(new Options
            {
                UseHttps = ApiConstants.UseHttps,
                UrlRoot = ApiConstants.Url
            });
            builder.RegisterInstance(clientFactory).As<IClientFactory>();
            
            var notificationService = new NotificationService();
            builder.RegisterInstance(notificationService).As<INotificationService>();
        }
    }
}