using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    public class NotificationsViewModel : BaseViewModel
    {
        private readonly IClientFactory _clientFactory;
        private Guid _deviceId;

        private IEnumerable<NotificationMessageDto> _notifications;
        public IEnumerable<NotificationMessageDto> Notifications
        {
            get
            {
                return _notifications;
            }
            set
            {
                _notifications = value;
                NotifyPropertyChanged();
            }
        }
        
        public NotificationsViewModel()
        {
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();

            if (Application.Current.Properties.Any(x => x.Key == "devices"))
            {
                var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceDto>>((string)Application.Current.Properties["devices"]);
                _deviceId = devices.LastOrDefault().Id;
            }


            Init();
        }

        private async void Init()
        {
            await RefreshNotificationMessagesData();

            var timer = new Timer(5000);
            timer.Enabled = true;
            timer.Elapsed += async (object sender, ElapsedEventArgs e) =>
            {
                await RefreshNotificationMessagesData();
            };
            timer.Start();
        }

        public Command ClearCommand
        {
            get
            {
                return new Command(() =>
                {
                     _clientFactory.NotificationMessageClient.ConfirmNotificationMessages(_deviceId);
                });
            }
        }

        private async Task RefreshNotificationMessagesData()
        {
            try
            {
                var data = await _clientFactory.NotificationMessageClient.GetNotificationMessages(_deviceId);
                if (data != null)
                {
                    Notifications = data;
                    foreach(var not in Notifications)
                    {
                        not.TimeStamp = not.TimeStamp.ToLocalTime();
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO Add logger
            }
        }
    }
}
