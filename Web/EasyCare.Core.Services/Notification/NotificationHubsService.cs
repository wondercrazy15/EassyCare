using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyCare.Core.Services.Notification
{
    public class NotificationHubService : INotificationService
    {
        readonly NotificationHubClient _hub;
        readonly Dictionary<string, NotificationPlatform> _installationPlatform;
        readonly ILogger<NotificationHubService> _logger;

        public NotificationHubService(IOptions<NotificationHubOptions> options, ILogger<NotificationHubService> logger)
        {
            _logger = logger;
            //_hub = NotificationHubClient.CreateClientFromConnectionString(
            //    options.Value.ConnectionString,
            //    options.Value.Name);

            _hub = NotificationHubClient.CreateClientFromConnectionString(
                NotificationConstants.FullAccessConnectionString,
                NotificationConstants.NotificationHubName);


            _installationPlatform = new Dictionary<string, NotificationPlatform>
            {
                { nameof(NotificationPlatform.Apns).ToLower(), NotificationPlatform.Apns },
                { nameof(NotificationPlatform.Fcm).ToLower(), NotificationPlatform.Fcm }
            };
        }

        public async Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallationDto deviceInstallationDto, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(deviceInstallationDto?.InstallationId) ||
                string.IsNullOrWhiteSpace(deviceInstallationDto?.Platform) ||
                string.IsNullOrWhiteSpace(deviceInstallationDto?.PushChannel))
                return false;

            var installation = new Installation()
            {
                InstallationId = deviceInstallationDto.InstallationId,
                PushChannel = deviceInstallationDto.PushChannel,
                Tags = deviceInstallationDto.Tags
            };

            if (_installationPlatform.TryGetValue(deviceInstallationDto.Platform, out var platform))
                installation.Platform = platform;
            else
                return false;

            try
            {
                await _hub.CreateOrUpdateInstallationAsync(installation, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(installationId))
                return false;

            try
            {
                await _hub.DeleteInstallationAsync(installationId, token);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> RequestNotificationAsync(NotificationRequestDto notificationRequest, CancellationToken token)
        {
            if (notificationRequest.Type == NotificationTypeConstant.Message)
            {
                notificationRequest.Title = "📩 " + notificationRequest.Title;
            }
            if (notificationRequest.Type == NotificationTypeConstant.Drugs)
            {
                notificationRequest.Title = "💊 " + notificationRequest.Title;
            }
            if (notificationRequest.Type == NotificationTypeConstant.Event)
            {
                notificationRequest.Title = "📅 " + notificationRequest.Title;
            }

            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                (string.IsNullOrWhiteSpace(notificationRequest?.Text)) ||
                string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var androidPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.Android :
                PushTemplates.Generic.Android;

            var iOSPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.iOS :
                PushTemplates.Generic.iOS;

            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action,
                notificationRequest.Title);

            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action,
                notificationRequest.Title
                );

            try
            {
                if (notificationRequest.Tags.Length == 0)
                {
                    // This will broadcast to all users registered in the notification hub
                    await SendPlatformNotificationsAsync(androidPayload, iOSPayload, token);
                }
                else if (notificationRequest.Tags.Length <= 20)
                {
                    var result = await _hub.SendAppleNativeNotificationAsync(iOSPayload, notificationRequest.Tags, token);
                    //await SendPlatformNotificationsAsync(androidPayload, iOSPayload, notificationRequest.Tags, token);
                }
                else
                {
                    var notificationTasks = notificationRequest.Tags
                        .Select((value, index) => (value, index))
                        .GroupBy(g => g.index / 20, i => i.value)
                        .Select(tags => SendPlatformNotificationsAsync(androidPayload, iOSPayload, tags, token));

                    await Task.WhenAll(notificationTasks);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error sending notification");
                return false;
            }
        }

        private string PrepareNotificationPayload(string template, string text, string action, string title) => template
            .Replace("$(alertTitle)", title, StringComparison.InvariantCulture)
            .Replace("$(alertMessage)", text, StringComparison.InvariantCulture)
            .Replace("$(alertAction)", action, StringComparison.InvariantCulture);

        private Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, CancellationToken token)
        {
            var sendTasks = new Task[]
            {
                // Send Push-Notification for Android (not needed as long as we don't have configured the firbase service -> will lead to exception)
                //_hub.SendFcmNativeNotificationAsync(androidPayload, token),
                // Send Push-Notification for iOS
                _hub.SendAppleNativeNotificationAsync(iOSPayload, token)
            };

            return Task.WhenAll(sendTasks);
        }

        private Task SendPlatformNotificationsAsync(string androidPayload, string iOSPayload, IEnumerable<string> tags, CancellationToken token)
        {
            var sendTasks = new Task[]
                {
               // _hub.SendFcmNativeNotificationAsync(androidPayload, tags, token),
                _hub.SendAppleNativeNotificationAsync(iOSPayload, tags, token)
                };

            return Task.WhenAll(sendTasks);
        }

        public async Task<bool> SendcheduleNotificationAsync(NotificationScheduleRequestDto notificationRequest, CancellationToken token)
        {
            if (notificationRequest.Type == NotificationTypeConstant.Message)
            {
                notificationRequest.Title = "📩 " + notificationRequest.Title;
            }
            if (notificationRequest.Type == NotificationTypeConstant.Drugs)
            {
                notificationRequest.Title = "💊 " + notificationRequest.Title;
            }
            if (notificationRequest.Type == NotificationTypeConstant.Event)
            {
                notificationRequest.Title = "📅 " + notificationRequest.Title;
            }

            if ((notificationRequest.Silent &&
                string.IsNullOrWhiteSpace(notificationRequest?.Action)) ||
                (!notificationRequest.Silent &&
                (string.IsNullOrWhiteSpace(notificationRequest?.Text)) ||
                string.IsNullOrWhiteSpace(notificationRequest?.Action)))
                return false;

            var androidPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.Android :
                PushTemplates.Generic.Android;

            var iOSPushTemplate = notificationRequest.Silent ?
                PushTemplates.Silent.iOS :
                PushTemplates.Generic.iOS;

            var androidPayload = PrepareNotificationPayload(
                androidPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action,
                notificationRequest.Title);

            var iOSPayload = PrepareNotificationPayload(
                iOSPushTemplate,
                notificationRequest.Text,
                notificationRequest.Action,
                notificationRequest.Title
                );

            try
            {
                var notification = new AppleNotification(iOSPayload);
                var scheduled = await _hub.ScheduleNotificationAsync(notification, notificationRequest.ScheduleDate, notificationRequest.Tags,token);

                //var scheduled = await _hub.ScheduleNotificationAsync(notification, notificationRequest.ScheduleDate);
                return true;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error schedule notification");
                return false;
            }
        }

        public async Task<string> CreateOrUpdateRegistrationDevice(string deviceId, string userId)
        {
            try
            {
                string Tags = "";
                var registrationId = await _hub.CreateRegistrationIdAsync();
                if (registrationId != null)
                {
                    var registration = new AppleRegistrationDescription(deviceId)
                    {
                        RegistrationId = registrationId,  //one we got in previous  call.
                        Tags = new HashSet<string>
                        {
                             "username:" + userId
                        }
                    };

                    var result = await _hub.CreateOrUpdateRegistrationAsync(registration);
                    if (result != null)
                    {
                        Tags = result.Tags.FirstOrDefault();
                        return Tags;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error schedule notification");
                return null;
            }
        }

        // DELETE api/register/5
        public async Task<bool> DeleteRegistrationDevice(string DeviceCode, string NotificationTagCode)
        {
            try
            {

                var registrations = await _hub.GetRegistrationsByChannelAsync(DeviceCode, 100);
                foreach (var item in registrations)
                {
                    var tags = item.Tags.Where(x => x == NotificationTagCode).FirstOrDefault();
                    if (!string.IsNullOrEmpty(tags))
                    {
                        await _hub.DeleteRegistrationAsync(item);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error schedule notification");
                return false;
            }

        }
    }
}