using System;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Settings
{
    public class KontoPageViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private IClientFactory _clientFactory;
      
        public KontoPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
            var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);
            Imagepath = GlobalConstant.Url + "/EasyCare/User/"+logininfo.supervisor.Id+ ".jpg";
            SupervisorName = Application.Current.Properties["supervisor_name"].ToString();
        }

        private string _supervisorName;
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

        public async  void UpdateProfile(string image)
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("Login_info") && Application.Current.Properties["Login_info"] != null)
                {
                    var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);
                    var supervisor = logininfo.supervisor;
                    supervisor.Image = image;
                    var user = await _clientFactory.SupervisorClient.PutItem(supervisor.Id, supervisor);
                    if (user != null)
                    {
                        await CachedImage.InvalidateCache(Imagepath, CacheType.All, true);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
