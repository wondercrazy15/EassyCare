using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Converters;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.Views.UserAuthentication
{
    public partial class AddSeniorPage : ContentPage
    {
        private IClientFactory _clientFactory;

        public UserDto SupervisorItem { get; set; }

        public AddSeniorPage()
        {
            InitializeComponent();
            SupervisorItem = new UserDto();
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
        }

        // TODO Move this code to view models

        protected override void OnAppearing()
        {
            // UserName.Focus();
            base.OnAppearing();
        }

        void UserName_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SupervisorItem.SecondName = UserName.Text;
        }

        async void Email_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (Email.Text.Length > 8)
            {
                if (CheckValidEmail(Email.Text))
                {
                    SupervisorItem.EMail = Email.Text;

                }
                else
                {

                }

            }
        }

        void Password_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        private async void AddSeniorButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (CheckValidEmail(Email.Text.Trim()))
                {
                    if (Password.Text.Length > 8)
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        UserDto userItem = new UserDto();
                        userItem.Id = new Guid(Application.Current.Properties["supervisor_id"].ToString());
                        userItem.EMail = SupervisorItem.EMail;
                        userItem.Code = SupervisorItem.Code;
                        userItem.FirstName = SupervisorItem.FirstName;
                        userItem.SecondName = SupervisorItem.SecondName;
                        userItem.PwHash = Password.Text.Trim();

                        var obj = await _clientFactory.UserClient.AddSenior(userItem);

                        if (obj != null)
                        {

                            Application.Current.Properties["senior"] = JsonConvert.SerializeObject(obj);

                            Application.Current.Properties["senior_id"] = obj.Id;
                            var logininfo = JsonConvert.DeserializeObject<UsersDto>(Application.Current.Properties["Login_info"].ToString());
                            logininfo.senior = new SeniorDto()
                            {
                                FirstName = obj.FirstName,
                                Id = obj.Id,
                                SecondName = obj.SecondName
                            };
                            Application.Current.Properties["Login_info"] = JsonConvert.SerializeObject(logininfo);
                            Application.Current.SavePropertiesAsync();
                            DependencyService.Get<IToast>().Show("Senior erfolgreich angelegt");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            DependencyService.Get<IToast>().Show("Die E-Mail ist bereits registriert");
                        }
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        DependencyService.Get<IToast>().Show("Das Passwort muss mindestes 8 Zeichen lang sein, sowie eine Kombination aus Groß- und Kleinbuchstaben und Zahlen enthalten.");
                    }

                }
                else
                {
                    DependencyService.Get<IToast>().Show("Bitte geben Sie eine gültige E-Mail Adresse ein");
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.Message.ToString());
                UserDialogs.Instance.HideLoading();
            }


        }


        private static bool CheckValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            return EmailToBooleanConverter.IsValidEmail(email);
        }



        void Vorname_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SupervisorItem.FirstName = Vorname.Text;
        }
    }
}
