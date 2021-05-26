using System;
using System.Collections.Generic;
using System.Net;
using Foundation;
using WatchKit;
using HealthKit;
using System.Timers;
using SimpleConnection.WatchOSExtension.Models;
using SimpleConnection.WatchOSExtension.AzureServices;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using Microsoft.Azure.NotificationHubs;
using SimpleConnection.WatchOSExtension.Services;
using Xamarin.Essentials;

namespace SimpleConnection.WatchOSExtension
{
    public partial class InterfaceController : WKInterfaceController
    {
        private DeviceDto _device;
        private TANDto _tan;
        private NotificationMessageDto _lastNotificationMessage;

        private IClientFactory _clientFactory;
        private ISensorService _sensorService;
        private Dictionary<string, Timer> _timers;
        
        protected InterfaceController(IntPtr handle) : base(handle)
        {
            _clientFactory = ClientFactory.Create(new Options
            {
                UseHttps = ApiConstants.UseHttps,
                UrlRoot = ApiConstants.Url
            });
            _sensorService = new SensorService();
            _timers = new Dictionary<string, Timer>();
        }

        // Main context, handle main program logic
        public override void Awake(NSObject context)
        {
            base.Awake(context);
            
            _sensorService.ValidateAuthorization();
        }
        
        async partial void EmergencyButtonClick()
        {
            
            await SendNotificationMessage();
        }

        private async Task<NotificationMessageDto> SendNotificationMessage()
        {
            try
            {
                if (_device is null)
                {
                    _device = new DeviceDto(); ;
                    _device = await _clientFactory.DeviceClient.PostItem(_device);
                }

                var messageDto = new NotificationMessageDto
                {
                    DeviceId = _device.Id,
                    Description = "Hilfe",
                    NotificationId = Guid.NewGuid(),
                    TimeStamp = DateTime.Now
                };

                messageDto = await _clientFactory.NotificationMessageClient.PostItem(messageDto);

                PresentAlertController("Notruf abgesetzt!", "Hilfe ist auf dem Weg", WKAlertControllerStyle.Alert, new WKAlertAction[] { WKAlertAction.Create("OK", WKAlertActionStyle.Default, () => { }) });

                // TODO Add checking network connection and store not sended message
                return messageDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ShowAlert("Error", "Notification kann nicht gesendet werden!", () => { });
            }
            return null;
        }

        async partial void PairButtonClick()
        {
            if (_device is null)
            {
                _device = new DeviceDto();;
                _device = await _clientFactory.DeviceClient.PostItem(_device);
            }

            var rand = new Random();
            _tan = new TANDto
            {
                Expiration = DateTime.Now.AddMinutes(10),
                DeviceId = _device.Id,
                Code = rand.Next(1000, 9999).ToString()
            };
            _tan = await _clientFactory.TanClient.PostItem(_tan);

            // TODO this should be processed in the cloud
            // CreateAndStartTimer(1000 * 30, TanExpiredHandler);
            
            ShowAlert(_tan.Code, 
                "Bitte geben Sie die oben angezeigte TAN in Ihrer Smartphone - App ein",
                        TanConfirmed);
        }

        private void CreateAndStartTimer(string key, int interval, ElapsedEventHandler handler)
        {
            var timer = new Timer(interval);
            timer.Enabled = true;
            timer.Elapsed += handler;
            timer.Start();
            
            _timers.Add(key, timer);
        }

        private SensorMessageDto ReadSensorData()
        {
            var messageDto = new SensorMessageDto();
            
            messageDto.BatterySoC = _sensorService.ReadBatteryLevel();
            messageDto.BatteryIsCharge = _sensorService.BatteryIsCharge();
            messageDto.StepCount = _sensorService.ReadStepCount();
            messageDto.Location = _sensorService.ReadLocation();
            messageDto.HeartRate = (int)_sensorService.ReadHeartRate();
            messageDto.DeviceId = _device.Id;
            messageDto.Date = DateTime.UtcNow;
            
            return messageDto;
        }
        
        private async void TanExpiredHandler(object sender, ElapsedEventArgs e)
        {
            _tan = await _clientFactory.TanClient.DeleteItem(_tan.TanId);
        }
        
        private async void TanConfirmed()
        {
            var deviceDB = await _clientFactory.DeviceClient.GetItem(_device.Id);
            if (deviceDB.SeniorId != Guid.Empty && deviceDB.SupervisorId != Guid.Empty)
            {
                _device = deviceDB;
                CreateAndStartTimer("readDataTimer", 10000, (async (o, args) =>
                {
                    await SendSensorData();
                }));
            }
        }

        private async Task<SensorMessageDto> SendSensorData()
        {
            try
            {
                var messageDto = ReadSensorData();
                messageDto = await _clientFactory.SensorMessagesClient.PostItem(messageDto);
                // TODO Add checking network connection and store not sended message
                return messageDto;
            }
            catch (Exception e)
            {
                ShowAlert("Error", "Sensordaten können nicht gelesen oder gesendet werden!", () => { });
            }
            return null;
        }

        private void ShowAlert(string title, string message, Action handle)
        {
            PresentAlertController(title, 
                message, 
                WKAlertControllerStyle.Alert, 
                new WKAlertAction[]
                {
                    WKAlertAction.Create("OK", WKAlertActionStyle.Default, handle)
                });
        }
        
        private static async Task SendTemplateNotificationsAsync(string message)
        {
            var hub = NotificationHubClient.CreateClientFromConnectionString(
                NotificationConstants.FullAccessConnectionString, NotificationConstants.NotificationHubName);
            var templateParameters = new Dictionary<string, string>();
            
            foreach (var tag in NotificationConstants.SubscriptionTags)
            {
                templateParameters["messageParam"] = message;
                try
                {
                    // Define an iOS alert..
                    var iOSalert =
                                "{\"aps\":{\"alert\":\"Hello. This is a iOS notification! Tada!\", \"sound\":\"default\"}}";

                    // ..And send it
                    hub.SendAppleNativeNotificationAsync(iOSalert).Wait();
                    Console.WriteLine($"Send:");

                    //await hub.SendTemplateNotificationAsync(templateParameters, tag);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Failed to send template notification: {ex.Message}");
                }
            }
        }
    }
}

