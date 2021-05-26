using System;
using System.Collections.Generic;

using Xamarin.Forms;
using EasyCare.Views.Dashboard;
using EasyCare.Core.Dto;
using EasyCare.Client;
using EasyCare.DI;
using Autofac;
using Newtonsoft.Json;
using System.Linq;

namespace EasyCare
{
    public partial class PairPage : ContentPage
    {
        private SeniorDto senior;
        private SupervisorDto supervisor;
        private IClientFactory _clientFactory;

        public PairPage(SupervisorDto supervisor, SeniorDto senior)
        {
            InitializeComponent();
            this.supervisor = supervisor;
            this.senior = senior;
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
        }

        void WatchTAN_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (WatchTAN_Editor.Text.Length == 4)
            {
                PairButton.IsVisible = true;
            }
            else
            {
                PairButton.IsVisible = false;
            }
        }

        async void PairButton_Pressed(System.Object sender, System.EventArgs e)
        {
            PairButton.IsEnabled = false;

            var code = WatchTAN_Editor.Text;
            var tan = await _clientFactory.TanClient.GetByCode(code);
            if(tan is null)
            {
                PairPageLabel.TextColor = Color.FromHex("CE2552");
                PairPageLabel.Text = "Die Eingegebene TAN ist ungültig, bitte probiere es erneut!";
                PairButton.IsEnabled = false;
                return;
            }

            var device = await _clientFactory.DeviceClient.GetItem(tan.DeviceId);
            if (device is null)
            {
                PairingResultLabel.Text = "Gerät kann nicht gefunden werden";
                return;
            }

            device.SeniorId = senior.Id;
            device.SupervisorId = supervisor.Id;
            device = await _clientFactory.DeviceClient.PutItem(device.Id, device);

            if ((device.SeniorId == senior.Id) && (device.SupervisorId == supervisor.Id))
            {
                PairButton.IsVisible = false;
                PairingResultLabel.Text = "Erfolgreich gekoppelt!";

                var devices = new List<DeviceDto> { device };

                Application.Current.Properties["devices"] = JsonConvert.SerializeObject(devices);
                

                MainMenuButton.IsVisible = true;
            }
            else
            {
                PairingResultLabel.Text = String.Format("Ein unbekannter Fehler ist aufgetreten!");
                PairButton.IsEnabled = false;
            }
            PairingResultLabel.IsVisible = true;
        }

        async void MainMenuButton_Pressed(System.Object sender, System.EventArgs e)
        {
            if (Application.Current.Properties.Any(x => x.Key == "devices"))
            {
                var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceDto>>((string)Application.Current.Properties["devices"]);
                var device = devices.SingleOrDefault();
                var senior = await _clientFactory.SeniorClient.GetItem(device.SeniorId);

                await Navigation.PushModalAsync(new MainPage(senior, supervisor, device.Id));
            }
            else
            {
                PairingResultLabel.Text = String.Format("Schließen Sie den Pairing-Vorgang ab!");
            }
        }
    }
}
