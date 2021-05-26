using System;
using System.Threading.Tasks;
using System.Timers;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    /// <summary>
    /// ViewModel for chat message page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MonitoringOverviewViewModel : BaseViewModel
    {
        private SensorMessageDto _messageDto;
        private SeniorDto _senior;
        private IClientFactory _clientFactory;
        private Guid _deviceId;
        INavigation _navigation;
        public MonitoringOverviewViewModel(INavigation navigation, SeniorDto senior, Guid deviceId)
        {
            _navigation = navigation;
            _senior = senior;
            _deviceId = deviceId;
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();

            if(senior!=null)
            {
                Imagepath = GlobalConstant.Url + "/EasyCare/User/" + _senior.Id + ".jpg";
            }
            else
            {
                Imagepath = GlobalConstant.Url;
            }
            

            Init();
        }


        public SensorMessageDto Sensors
        {
            get
            {
                return _messageDto;
            }
            set
            {
                _messageDto = value;
                NotifyPropertyChanged();
            }
        }

        private string _Imagepath;
        public string Imagepath
        {
            get
            {
                return _Imagepath;
            }
            set
            {
                _Imagepath = value;
                NotifyPropertyChanged();
            }
        }

        public SeniorDto Senior
        {
            get
            {
                return _senior;
            }
            set
            {
                _senior = value;
                NotifyPropertyChanged();
            }
        }

        private async void Init()
        {
            await RefreshSensorData();
            var timer = new Timer(10000);
            timer.Enabled = true;
            timer.Elapsed += async (object sender, ElapsedEventArgs e) =>
            {
                await RefreshSensorData();
            };
            timer.Start();
        }

        private async Task RefreshSensorData()
        {
            try
            {
                var data = await _clientFactory.SensorMessagesClient.GetItem(_deviceId);
                if (data != null)
                {
                    Sensors = data;
                }
            }
            catch(Exception ex)
            {
                // TODO Add logger
            }
        }
    }
}
