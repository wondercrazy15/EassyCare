using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.ViewModels.BottomNavigation;
using EasyCare.ViewModels.Dashboard.Settings;
using Newtonsoft.Json;
using Xamarin.Forms;
using SettingsViewModel = EasyCare.ViewModels.Dashboard.Settings.SettingsViewModel;

namespace EasyCare.Views.Dashboard.Settings
{
   // SettingsViewModel _vm;
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Dashboard.Settings.SettingsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);
            (BindingContext as SettingsViewModel).Imagepath = GlobalConstant.Url + "/EasyCare/User/" + logininfo.supervisor.Id + ".jpg";
           
        }
    }
}
