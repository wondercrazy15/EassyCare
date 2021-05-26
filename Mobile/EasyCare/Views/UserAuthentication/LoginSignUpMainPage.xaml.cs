using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Autofac;
using EasyCare.B2C;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Services;
using EasyCare.Views.Dashboard;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.Views.UserAuthentication
{
    public partial class LoginSignUpMainPage : ContentPage
    {
        private IClientFactory _clientFactory;
        public LoginSignUpMainPage()
        {

            InitializeComponent();
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
        }

        async void LoginButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
               
                await Navigation.PushModalAsync(new LogInPage());
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
          


        }

        async void RegisterButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUpPage());
        }
    }
}
