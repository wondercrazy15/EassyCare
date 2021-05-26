using System;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class NotificationsClient : BaseClient, INotificationsClient
    {
        public NotificationsClient(Options options) : base(options, "Notifications")
        {
        }
        
        public async Task UpdateInstallation(DeviceInstallationDto deviceInstallationDto)
        {
            await Put<DeviceInstallationDto, DeviceInstallationDto>(
                "installations", 
                String.Empty,
                deviceInstallationDto);
        }

        public async Task DeleteInstallation(string installationId)
        {
            await Delete("installations", $"installationId={installationId}");
        }

        public async Task RequestPush(NotificationRequestDto notificationRequest)
        {
            await Post<NotificationRequestDto, NotificationRequestDto>("requests", notificationRequest);
        }
    }
}