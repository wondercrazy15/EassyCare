using System;
using EasyCare.Core.Dto;
using EasyCare.Views.Dashboard;
using EasyCare.Views.Pairing;
using EasyCare.Views.UserAuthentication;
using Xamarin.Forms;

namespace EasyCare
{
    public partial class UserPage : ContentPage
    {
        private SupervisorDto user = new SupervisorDto();

        public SupervisorDto User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }

        public UserPage(SupervisorDto supervisor)
        {
            InitializeComponent();
            User = supervisor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            WelcomeLabel.Text = String.Format("Registrierung erfolgreich! Sie können sich mit dieser E-Mail-Adresse nun anmelden.");
            //UserLabel.Text = String.Format("Name: {0} {1}\nEMail: {2}", User.FirstName, User.SecondName, User.EMail);
        }

        async void MainScreenButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
            await App.Current.MainPage.Navigation.PopToRootAsync();

            //Application.Current.MainPage = new MainPage();
            //await Navigation.PopToRootAsync();
        }

        async void ConfirmButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginSignUpMainPage());
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        

        async void CreateSeniorButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new CreateSeniorPage(user));
        }
    }
}
