using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Models.Chat;
using EasyCare.Views.Dashboard.Chat;
using Syncfusion.XForms.Chat;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Chat
{
    public class ChatMainViewModel : BaseViewModel
    {
        private Author _currentUser;
        private readonly IGroupClient _client;
        public SupervisorDto _author;
        public ObservableCollection<ChatUserList> usercollection = new ObservableCollection<ChatUserList>();
        private INavigation _navigation;
        private string v;
        private GroupDto result;

        public ChatMainViewModel(SupervisorDto supervisor)
        {
            try
            {
                _client = AppContainer.Container.Resolve<IClientFactory>().GroupClient;
                _author = supervisor;
                CurrentUser = new Author
                {
                    Name = $"{supervisor.FirstName} {supervisor.SecondName}"
                };
                IsVisibleAddMember = true;
            }
            catch (Exception ex)
            {

            }
        
        }

        public ChatMainViewModel(GroupDto result)
        {
            this.result = result;

            Application.Current.Properties["group_code"] = result.Code;
            Application.Current.SavePropertiesAsync();

            usercollection.Add(new ChatUserList
            {
                GroupName = result.Name,
                LastMessage = "",
                Time = "9:17",
                GroupId=result.Id.ToString(),
                ImagePath = $"{GlobalConstant.Url}{result.Image}"
            });
        }

        public async void AddGroupList()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                var supervisorid = new Guid(Application.Current.Properties["supervisor_id"].ToString());
                var group = await _client.GetGroupsBySupervisorId(supervisorid) ;
                Application.Current.Properties["group_code"] = group.Code;
                Application.Current.SavePropertiesAsync();
                if (string.IsNullOrEmpty(group.Code))
                    IsVisibleAddMember = true;
                else
                    IsVisibleAddMember = false;
                usercollection.Clear();
                usercollection.Add(new ChatUserList()
                {
                    GroupName = group.Name,
                    LastMessage = "",
                    GroupId=group.Id.ToString(),
                    Time = "",
                    Code=group.Code,
                    Isactive=group.Isactive,
                    SupervisorId=group.SupervisorId,
                    CreatedDate=group.CreatedDate,
                    ModifiedDate=group.ModifiedDate,
                    ImagePath= $"{GlobalConstant.Url}{group.Image}" 
                });

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        
        private bool isVisibleAddMember;
        public bool IsVisibleAddMember
        {
            get
            {
                return isVisibleAddMember;
            }
            set
            {
                isVisibleAddMember = value;
                NotifyPropertyChanged();
            }
        }

        public Author CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }

    }
}
