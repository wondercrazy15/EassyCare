using System;
using EasyCare.Views.Dashboard;
using Xamarin.Forms;
using EasyCare.Client.Contact;
using EasyCare.DI;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Dto;

namespace EasyCare.Views.Pairing
{
    public partial class CreateSeniorPage : ContentPage
    {
        private ISeniorClient _client;

        private bool postCodeValid = false;
        private bool cityValid = false;
        private bool streetValid = false;
        private bool houseNumberValid = false;

        SeniorDto senior = new SeniorDto();
        SupervisorDto supervisor;

        public CreateSeniorPage(SupervisorDto supervisor)
        {
            InitializeComponent();
            this.supervisor = supervisor;
            this._client = AppContainer.Container.Resolve<IClientFactory>().SeniorClient;
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void UserName_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (UserName.Text.Length > 8 && UserName.Text.Contains(" "))
            {
                string[] name = UserName.Text.Split(' ');
                senior.FirstName = name[0];
                senior.SecondName = name[1];

                AddressLabel.IsVisible = true;
                StreetEditor.IsVisible = true;
                CityEditor.IsVisible = true;
            }
            else
            {
                AddressLabel.IsVisible = false;
                StreetEditor.IsVisible = false;
                CityEditor.IsVisible = false;
            }
        }

        void StreetName_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if(StreetName.Text.Length > 5)
            {
                senior.Street = StreetName.Text;
            }
        }

        void HouseNumber_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e) =>
            NumberChanged(HouseNumber.Text, (x) =>
            {
                senior.Street += $" {x}";
            });

        void PostCode_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e) =>
            NumberChanged(PostCode.Text, (x) =>
            {
                senior.PostCode = x;
            });

        void CityName_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CityName.Text))
            {
                senior.City = CityName.Text;
            }
        }

        async void CreateSeniorButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                CreateSeniorButton.IsEnabled = false;
                senior = await _client.PostItem(senior);
                CreateSeniorButton.IsVisible = false;
                MainScreenButton.IsVisible = true;
                AddDeviceButton.IsVisible = true;
            }
            catch (Exception ex)
            {
                // TODO Add logger to store logs in Azure and move texts to resources
            }
        }

        async void AddDeviceButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new PairPage(supervisor, senior));
        }

        async void MainScreenButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }


        private bool IsAddressValid()
        {
            return (postCodeValid && cityValid && houseNumberValid && streetValid);
        }

        private void NumberChanged(string text, Action<int> action)
        {
            if (!string.IsNullOrEmpty(text) && int.TryParse(text, out int noValue))
            {
                action(noValue);
            }
        }
    }
}
