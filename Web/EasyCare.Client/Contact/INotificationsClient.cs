using System.Threading.Tasks;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface INotificationsClient
    {
        Task UpdateInstallation(DeviceInstallationDto deviceInstallationDto);

        Task DeleteInstallation(string installationId);

        Task RequestPush(NotificationRequestDto notificationRequest);
    }
}