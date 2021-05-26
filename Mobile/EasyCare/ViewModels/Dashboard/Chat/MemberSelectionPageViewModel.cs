using System;
using System.Collections.ObjectModel;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Settings;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Chat
{
    public class MemberSelectionPageViewModel : BaseViewModel
    {

        private IGroupClient _client;
        private readonly INavigation _navigation;
        private string _supervisorName;

        private ObservableCollection<MemberModel> usercollection = new ObservableCollection<MemberModel>();

        public MemberSelectionPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _client = AppContainer.Container.Resolve<IClientFactory>().GroupClient;
        }


        public bool _IsShowError;
        public bool IsShowError
        {
            get
            {
                return _IsShowError;
            }

            set
            {
                _IsShowError = value;
                NotifyPropertyChanged();
            }
        }

        public string _GroupName;
        public string GroupName
        {
            get
            {
                return _GroupName;
            }

            set
            {
                _GroupName = value;
                IsShowError = false;
                NotifyPropertyChanged();
            }
        }

        public string _GroupKey;
        public string GroupKey
        {
            get
            {
                return _GroupKey;
            }

            set
            {
                _GroupKey = value;
                IsShowError = false;
                NotifyPropertyChanged();
            }
        }

        public byte[] _GroupImage;
        private ITwilioChatHelper twilioChatHelper;

        public byte[] GroupImage
        {
            get
            {
                return _GroupImage;
            }

            set
            {
                _GroupImage = value;
                NotifyPropertyChanged();
            }
        }

        public Command CreateGroup
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (string.IsNullOrEmpty(GroupName) || string.IsNullOrEmpty(GroupKey))
                        {
                            IsShowError = true;
                        }
                        else
                        {
                            UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                            IsShowError = false;
                            var name = GroupName;
                            var key = GroupKey;
                            string aimage = "";
                            if (GroupImage!=null)
                            {
                                aimage = Convert.ToBase64String(GroupImage); 
                            }

                            GroupDto group = new GroupDto();
                            group.Image = aimage;
                            group.Code = GroupKey;
                            group.Name = GroupName;
                            group.SupervisorId = Application.Current.Properties["supervisor_id"].ToString();
                            GroupDto result = await _client.PostItem(group);
                            if (result.Id != Guid.Empty)
                            {
                                twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                                twilioChatHelper.CreateChannel(result.Id.ToString());
                                UserDialogs.Instance.HideLoading();
                                ChatMainViewModel chatmainpage = new ChatMainViewModel(result);
                                UserDialogs.Instance.HideLoading();
                                await _navigation.PopAsync();
                            }
                            else
                            {
                                DependencyService.Get<IToast>().Show("Group already exist");
                            }
                        }
                        
                        UserDialogs.Instance.HideLoading();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
        }

    }
}
