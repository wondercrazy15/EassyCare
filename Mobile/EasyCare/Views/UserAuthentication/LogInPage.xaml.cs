using System;
using System.Linq;
using System.Security.Cryptography;
using Acr.UserDialogs;
using Autofac;
using EasyCare.B2C;
using EasyCare.Client;
using EasyCare.Converters;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.Crypto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Services;
using EasyCare.Views.Dashboard;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.Views.UserAuthentication
{
    public partial class LogInPage : ContentPage
    {
        private IClientFactory _clientFactory;
        private SignedUpUsersDto _user;
        private bool _emailValid;
        private bool _passwordValid;


        public LogInPage()
        {
            InitializeComponent();

            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
            _user = new SignedUpUsersDto();
            _emailValid = false;
            _passwordValid = false;
            //Email.Text = "test@gmail.com";
            //Password.Text = "123456789";
        }

        void Email_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //if (Email.Text.Length > 8)
            //{
            //    if (EmailToBooleanConverter.IsValidEmail(Email.Text))
            //    {
            //        _user.EMail = Email.Text;
            //        _emailValid = true;
            //      //  LogInButton.IsVisible = IsLogInReady();
            //        ErrorMessage("Falsche email", false);
            //    }
            //    else
            //    {
            //        ErrorMessage("Das Passwort ist zu kurz!", true);
            //    }

            //}
        }

        void Password_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //if (Password.Text.Length > 8)
            //{
            //    _passwordValid = true;
            //  //  LogInButton.IsVisible = IsLogInReady();
            //    ErrorMessage("", false);
            //}
            //else
            //{
            //    ErrorMessage("Das Passwort ist zu kurz!", true);
            //}
        }

        async void LogInButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                ErrorMessage("", false);
                //  DisableLogInButton();

                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);

                LoginDto loginDto = new LoginDto();
                loginDto.Email = Email.Text;
                loginDto.Password = Password.Text.Trim();

                var obj=  await _clientFactory.TokenClient.Token(loginDto);
                if(obj!=null)
                {
                    if(obj.AccessToken!=null)
                    {
                        Application.Current.Properties["Token"] = obj.AccessToken;
                        Application.Current.Properties["Email"] = Email.Text;
                        GlobalConstant.AccessToken = obj.AccessToken;

                        var customSupervisors = await _clientFactory.SupervisorClient.GetSupervisorByEmail(Email.Text, GlobalConstant.deviceID == null ? "123" : GlobalConstant.deviceID);
                        Application.Current.Properties["Login_info"] = JsonConvert.SerializeObject(customSupervisors);
                        Application.Current.Properties["supervisor_name"] = $"{customSupervisors.supervisor.FirstName} {customSupervisors.supervisor.SecondName}";
                        Application.Current.Properties["supervisor_id"] = customSupervisors.supervisor.Id;
                        Application.Current.Properties["devices"] = JsonConvert.SerializeObject(customSupervisors.device);
                        Application.Current.Properties["registered_device_code"] = customSupervisors.device.NotificationTagCode;

                        if (customSupervisors.senior != null)
                        {
                            Application.Current.Properties["senior_id"] = customSupervisors.senior.Id;
                        }

                        Application.Current.SavePropertiesAsync();

                        //Get Twilio Token
                        TwilioToken twilioToken = new TwilioToken();
                        //UserSettings.TwilioToken.Replace("\"", string.Empty);
                        var token = twilioToken.getTwilioToken(customSupervisors.supervisor.EMail);
                        ITwilioChatHelper twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                        twilioChatHelper.CreateClient(token.Replace("\"", string.Empty), Application.Current.Properties["Email"].ToString());

                        await Navigation.PushModalAsync(new MainPage(customSupervisors.senior, customSupervisors.supervisor, customSupervisors.device.Id));
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        DependencyService.Get<IToast>().Show("Invalid Username and Password");
                    }

                }
                else
                {
                    UserDialogs.Instance.HideLoading();

                    DependencyService.Get<IToast>().Show("Invalid Username and Password");

                }
            }
            catch (Exception ex)
            {
                // TODO Add logger to store logs in Azure and move texts to resources
                UserDialogs.Instance.HideLoading();
                ErrorMessage("Kann nicht einloggen! Sind Sie sicher, dass Ihr Konto existiert?", true);
            }
        }

        async void SignUpButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUpPage());
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void DisableLogInButton()
        {
            LogInButton.Text = "...";
            LogInButton.TextColor = Color.FloralWhite;

        }

        private bool IsLogInReady()
        {
            if (_emailValid && _passwordValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ErrorMessage(string text, bool isVisible)
        {
            LoginSuccessfullLabel.Text = text;
            LoginSuccessfullLabel.IsVisible = isVisible;
        }

    }
}
