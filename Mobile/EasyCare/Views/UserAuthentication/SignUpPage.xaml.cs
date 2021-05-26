using System;
using Xamarin.Forms;
using System.Security.Cryptography;
using EasyCare.Crypto;
using EasyCare.Converters;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;

namespace EasyCare.Views.UserAuthentication
{
    public partial class SignUpPage : ContentPage
    {
        private IClientFactory _clientFactory;

        public UserDto SupervisorItem { get; set; }

        public SignUpPage()
        {
            InitializeComponent();

            //BindingContext = new SignUpPageViewModel();
            SupervisorItem = new UserDto();
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
        }

        // TODO Move this code to view models

        protected override void OnAppearing()
        {
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

        private async void SignUpButton_Clicked(System.Object sender, System.EventArgs e)
        {
            if (CheckValidEmail(Email.Text.Trim()))
            {
                if(Password.Text.Length > 8)
                {
                    SignUpButton.Text = "...";
                    SignUpButton.IsEnabled = false;

   
                    UserDto userItem = new UserDto();
                    userItem.EMail = SupervisorItem.EMail;
                    userItem.Code = SupervisorItem.Code;
                    userItem.FirstName = SupervisorItem.FirstName;
                    userItem.SecondName = SupervisorItem.SecondName;
                    userItem.PwHash = Password.Text.Trim();

                    userItem = await _clientFactory.UserClient.PostItem(userItem);

                    if(userItem!=null)
                    {
                        SupervisorDto supervisorDto = new SupervisorDto();
                        supervisorDto.EMail = SupervisorItem.EMail;
                        supervisorDto.FirstName = SupervisorItem.FirstName;
                        supervisorDto.SecondName = SupervisorItem.SecondName;
                        await Navigation.PushModalAsync(new UserPage(supervisorDto));
                    }
                    else
                    {
                        DependencyService.Get<IToast>().Show("Die E-Mail ist bereits registriert");
                    }
                   
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
        

           
        }

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="email">Gets the email</param>
        /// <returns>Returns the boolean value.</returns>
        private static bool CheckValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            return EmailToBooleanConverter.IsValidEmail(email);
        }

        async void LogInButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new LogInPage());
        }

        void ExistingFrame_Tapped(System.Object sender, System.EventArgs e)
        {
            ExistingFrame.BackgroundColor = Color.FromHex("#CF3352");
            lblExisting.TextColor = Color.White;
            lblExistingGroup.TextColor = Color.White;


            NewFrame.BackgroundColor = Color.Transparent;
            lblNew.TextColor = Color.FromHex("#0A144F");
            lblNewGroup.TextColor = Color.FromHex("#0A144F");
            ExistingContainer.IsVisible = true;
        }

        void NewFrame_Tapped(System.Object sender, System.EventArgs e)
        {
            NewFrame.BackgroundColor = Color.FromHex("#CF3352");
            lblNew.TextColor = Color.White;
            lblNewGroup.TextColor = Color.White;


            ExistingFrame.BackgroundColor = Color.Transparent;
            lblExisting.TextColor = Color.FromHex("#0A144F");
            lblExistingGroup.TextColor = Color.FromHex("#0A144F");
            ExistingContainer.IsVisible = false;
        }

        async void BackButton_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void ExistingGroupkeyname_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SupervisorItem.Code = ExistingGroupkeyname.Text;
        }

        void Vorname_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SupervisorItem.FirstName = Vorname.Text;
        }
    }
}
