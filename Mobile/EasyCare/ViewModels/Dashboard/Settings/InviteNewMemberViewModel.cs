using System;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Chat;
using EasyCare.Models.Settings;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Settings
{
    public class InviteNewMemberViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private string _supervisorName;
        private ObservableCollection<MemberModel> usercollection = new ObservableCollection<MemberModel>();
        private IClientFactory _clientFactory;


        public InviteNewMemberViewModel(INavigation navigation)
        {
            try
            {
                _navigation = navigation;
                _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
                AddDataAsync().Wait(5);
            }
            catch (Exception ex)
            {

            }

        }

        private async System.Threading.Tasks.Task AddDataAsync()
        {
            try
            {

                var Participans = await _clientFactory.SupervisorClient.GetGroupParticipanById(new Guid(Application.Current.Properties["supervisor_id"].ToString()));
                if (Participans != null)
                {
                    foreach (var item in Participans)
                    {
                        Usercollection.Add(new MemberModel()
                        {
                            Id=item.Id.ToString(),
                            IsModerator=item.IsModerator,
                            IsSenior=item.IsSenior,
                            Email=item.EMail,
                            Name = item.FirstName + " " + item.SecondName,
                            Type = item.IsModerator ? "Administrator" : "Unterkonto"
                        }); ;
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        async internal void MakeModerator(MemberModel member)
        {
            try
            {
                SetUserDto setUser = new SetUserDto();
                setUser.GroupId = new Guid();
                setUser.ParticipantId = new Guid(member.Id);

                var obj = await _clientFactory.UserClient.SetModerator(setUser);
                if (obj != null)
                {
                    int existindex = usercollection.ToList().FindIndex(s => s.IsModerator);
                    var existingobj = usercollection[existindex];
                    usercollection.RemoveAt(existindex);
                    existingobj.IsModerator = false;
                    usercollection.Insert(existindex, existingobj);

                    int index= usercollection.ToList().FindIndex(s=>s.Id.Equals(member.Id));
                    usercollection.RemoveAt(index);

                    usercollection.Insert(index, new MemberModel()
                    {
                        Id = obj.Id.ToString(),
                        IsModerator = obj.IsModerator,
                        IsSenior = obj.IsSenior,
                        Email = obj.EMail,
                        Name = obj.FirstName + " " + obj.SecondName,
                        Type = obj.IsModerator ? "Administrator" : "Unterkonto"
                    });
                    DependencyService.Get<IToast>().Show("Administrator set successfully");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async void RemoveFromGroup(MemberModel member)
        {
            try
            {
                SetUserDto setUser = new SetUserDto();
                setUser.GroupId = new Guid();
                setUser.ParticipantId = new Guid(member.Id);

                var obj = await _clientFactory.UserClient.DeleteItem(setUser.ParticipantId);
                if (obj != null)
                {
                    int index = usercollection.ToList().FindIndex(s => s.Id.Equals(member.Id));
                    usercollection.RemoveAt(index);
                    DependencyService.Get<IToast>().Show("remove from group successfully");
                }
            }
            catch (Exception ex)
            {

            }
        }

        async internal void MakeSenior(MemberModel member)
        {
            try
            {
                 SetUserDto setUser = new SetUserDto();
                setUser.GroupId = new Guid();
                setUser.ParticipantId = new Guid(member.Id);

                var obj=  await _clientFactory.UserClient.SetSenior(setUser);
                if(obj!=null)
                {
                    Application.Current.Properties["senior"] = JsonConvert.SerializeObject(obj);
                 
                    Application.Current.Properties["senior_id"] = obj.Id;
                    var logininfo= JsonConvert.DeserializeObject<UsersDto>(Application.Current.Properties["Login_info"].ToString());
                    logininfo.senior = new SeniorDto()
                    {
                        FirstName = obj.FirstName, Id = obj.Id, SecondName = obj.SecondName
                    };
                    Application.Current.Properties["Login_info"] = JsonConvert.SerializeObject(logininfo);
                    Application.Current.SavePropertiesAsync();
                    DependencyService.Get<IToast>().Show("Senior set successfully");
                }
            }
            catch (Exception ex)
            {

            }
           
        }

        public ObservableCollection<MemberModel> Usercollection
        {
            get
            {
                return usercollection;
            }
            set
            {
                usercollection = value;
                NotifyPropertyChanged();
            }
        }


    }
}
