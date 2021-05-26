using System.Threading;
using System.Threading.Tasks;
using EasyCare.Core.Dto;

namespace EasyCare.Core.Services.Notification
{
    public interface INotificationService
    {
        Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallationDto deviceInstallationDto, CancellationToken token);
        Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token);
        Task<bool> RequestNotificationAsync(NotificationRequestDto notificationRequest, CancellationToken token);
        Task<bool> SendcheduleNotificationAsync(NotificationScheduleRequestDto notificationRequest, CancellationToken token);
        Task<string> CreateOrUpdateRegistrationDevice(string deviceId, string userId);
        Task<bool> DeleteRegistrationDevice(string DeviceCode, string NotificationTagCode);
    }
}