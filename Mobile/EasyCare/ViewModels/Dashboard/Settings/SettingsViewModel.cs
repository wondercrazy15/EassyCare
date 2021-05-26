using System;
using Acr.UserDialogs;
using Autofac;
using EasyCare.B2C;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Views.UserAuthentication;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private string _supervisorName;
        private bool _IsSeniorNotAvailable;
        

        private IClientFactory _clientFactory;

        public SettingsViewModel(INavigation navigation)
        {
            _navigation = navigation;
            SupervisorName = Application.Current.Properties["supervisor_name"].ToString();
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
            var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);
            Imagepath = GlobalConstant.Url + "/EasyCare/User/" + logininfo.supervisor.Id + ".jpg";

            if(logininfo.senior!=null)
            {
                IsSeniorNotAvailable = false;
            }
            else
            {
                IsSeniorNotAvailable = true;
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

        public string SupervisorName
        {
            get
            {
                return _supervisorName;
            }
            set
            {
                _supervisorName = value;
                NotifyPropertyChanged();
            }
        }


        public bool IsSeniorNotAvailable
        {
            get
            {
                return _IsSeniorNotAvailable;
            }
            set
            {
                _IsSeniorNotAvailable = value;
                NotifyPropertyChanged();
            }
        }

        
        public Command NotificationSettingCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PushAsync(
                        new Views.Dashboard.Settings.NotificationSettingPage());
                });
            }
        }
        public Command KontoCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PushAsync(
                        new Views.Dashboard.Settings.KontoPage());
                });
            }
        }

        public Command InviteNewMemberCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PushAsync(
                        new Views.Dashboard.Settings.InviteNewMemberPage());
                });
            }
        }

        public Command CreateSeniorCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var supervisiorId = (Guid?)Application.Current.Properties["supervisor_id"];
                    if (supervisiorId is null)
                    {
                        // TODO Add some message that cannot get supervisor id
                        return;
                    }

                    var client = AppContainer.Container.Resolve<IClientFactory>().SupervisorClient;
                    var supervisor = await client.GetItem(supervisiorId.Value);

                    await _navigation.PushAsync(
                        new Views.Pairing.CreateSeniorPage(supervisor));
                });
            }
        }


        public Command AddSeniorCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _navigation.PushAsync(new AddSeniorPage());
                });
            }
        }

        public Command LogoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        if (Application.Current.Properties.ContainsKey("registered_device_code") && Application.Current.Properties["registered_device_code"] != null)
                        {
                           await _clientFactory.TanClient.DeleteTANByCode(Application.Current.Properties["registered_device_code"].ToString());
                        }

                        Application.Current.Properties.Clear();
                        UserDialogs.Instance.HideLoading();
                        Application.Current.MainPage = new Views.UserAuthentication.LoginSignUpMainPage();
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Properties.Clear();
                        Application.Current.MainPage = new Views.UserAuthentication.LoginSignUpMainPage();
                        UserDialogs.Instance.HideLoading();
                    }

                });
            }
        }
    }
}
