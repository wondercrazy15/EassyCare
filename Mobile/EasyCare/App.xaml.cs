using System;
using System.IO;
using Xamarin.Forms;
using EasyCare.Data;
using EasyCare.DI;
using EasyCare.Views.UserAuthentication;
using EasyCare.Core.Constants;
using EasyCare.DataService;
using Twilio.Jwt.AccessToken;
using System.Collections.Generic;
using EasyCare.Services;
using EasyCare.B2C;
using EasyCare.Views.Dashboard.Chat;
using Newtonsoft.Json;
using EasyCare.Core.Dto;
using EasyCare.Views.Dashboard;
using EasyCare.Interface;

namespace EasyCare
{
    public partial class App : Application
    {
        public static string BaseImageUrl { get; } = SyncfusionConstants.ImageUrl;
        public static string ImageIdToSave = null;
        public static string DefaultImageId = "default_image";

        

        // Start Application
        public App()
        {
            try
            {
                AppContainer.Container = new AppSetup().CreateContainer();
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncfusionConstants.License);

                InitializeComponent();
                //Get Twilio Token
             //   TwilioToken twilioToken = new TwilioToken();
             //   var token= twilioToken.getTwilioToken("user@example.com");
                
                if (Application.Current.Properties.ContainsKey("Login_info") && Application.Current.Properties["Login_info"] != null)
                {
                    GlobalConstant.AccessToken = Application.Current.Properties["Token"].ToString();
                    var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);

                    //Get Twilio Token
                    TwilioToken twilioToken = new TwilioToken();
                    //UserSettings.TwilioToken.Replace("\"", string.Empty);
                    var token = twilioToken.getTwilioToken(logininfo.supervisor.EMail);
                    ITwilioChatHelper twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                    twilioChatHelper.CreateClient(token.Replace("\"", string.Empty),logininfo.supervisor.EMail);

                    MainPage = new MainPage(logininfo.senior, logininfo.supervisor, logininfo.device.Id);
                }
                else
                {
                    MainPage = new LoginSignUpMainPage();
                }
                DependencyService.Register<B2CAuthenticationService>();

            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            DependencyService.Get<ICalandarService>(DependencyFetchTarget.GlobalInstance).CreateService();
        }
    }
}
