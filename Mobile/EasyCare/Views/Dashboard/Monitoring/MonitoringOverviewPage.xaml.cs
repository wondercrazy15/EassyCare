using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.Models;
using EasyCare.ViewModels.Dashboard.Monitoring;
using EasyCare.Views.Dashboard.Settings;
using EasyCare.Views.UserAuthentication;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class MonitoringOverviewPage : ContentPage
    {
        public MonitoringOverviewPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(Application.Current.Properties.ContainsKey("senior"))
            {
               
                SeniorDto senior = JsonConvert.DeserializeObject<SeniorDto>(Application.Current.Properties["senior"].ToString());
                DeviceDto device = JsonConvert.DeserializeObject<DeviceDto>(Application.Current.Properties["devices"].ToString());

                var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);

                BindingContext = new MonitoringOverviewViewModel(Navigation, senior, device.Id);
              
                (BindingContext as MonitoringOverviewViewModel).Imagepath = GlobalConstant.Url + "/EasyCare/User/" + logininfo.senior.Id + ".jpg";
                Application.Current.Properties.Remove("senior");
            }

        }


        // TODO: This should be moved to the ViewControler
        async void Notifications_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NotificationsPage());
        }
        
        async void Location_Clicked(System.Object sender, System.EventArgs e)
        {
            var vm = BindingContext as MonitoringOverviewViewModel;
            if (vm.Sensors != null && !string.IsNullOrEmpty(vm.Sensors.Location))
            {
                var senior = vm.Senior;
                var locationArray = vm.Sensors.Location.Split(',').Select(x => double.Parse(x, CultureInfo.InvariantCulture));
                await Navigation.PushAsync(new MapPage(senior, locationArray.First(), locationArray.Last(), vm.Sensors.Date));
            }
        }

        async void Settings_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        async void SfButton_Clicked(System.Object sender, System.EventArgs e)
        {

            try
            {
                if (Application.Current.Properties["senior_id"] != null)
                    await Navigation.PushAsync(new MedikamentePage());
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("The given key 'senior_id' was not present in the dictionary."))
                {
                    await Navigation.PushAsync(new AddSeniorPage());
                }
            }

        }
    }
}