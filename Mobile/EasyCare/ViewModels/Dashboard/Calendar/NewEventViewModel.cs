using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Settings;
using EasyCare.Views.Dashboard.Calendar;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Calendar
{
    public class NewEventViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;

        public string Kategorieauswahlen { get; set; }
        public string Id { get; set; }


        private IClientFactory _clientFactory;
        public Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await _navigation.PopModalAsync();
                    }
                    catch (Exception ex)
                    {
                        // TODO Add logger
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }
        public Command memberPopup
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await PopupNavigation.Instance.PushAsync(new MemberListPopup(Usercollection, memberList));
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        var client = AppContainer.Container.Resolve<IClientFactory>().CalendarEventClient;
                        CalendarEventDto dto = new CalendarEventDto();

                        //dto.StartDate = (_selectedDateTime.Date + StartTime).ToMyFormatString();
                        //dto.EndDate = (EndDateTime.Date + endTime).ToMyFormatString();

                        dto.StartDate = (_selectedDateTime.Date + StartTime).ToString("yyyy-MM-dd HH:mm:ss");
                        dto.EndDate = (EndDateTime.Date + endTime).ToString("yyyy-MM-dd HH:mm:ss");

                        dto.Title = Title;

                        List<string> lstselectedMember = new List<string>();
                        foreach (var item in Usercollection)
                        {
                            if (item.IsSelected)
                            {
                                lstselectedMember.Add(item.Id);
                            }
                        }

                        dto.CalenderSupervisorId = lstselectedMember.ToArray();
                        dto.PeriodOfTime = selectedchoice != null ? selectedchoice : "None";
                        dto.EventSchedule = Selectedday != null ? Selectedday : "Never";
                        dto.Icon = Kategorieauswahlen;
                        dto.SupervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                        dto.EventScheduleDate = (_selectedDateTime.Date + StartTime).ToUniversalTime();
                        if (Id != null && Id != "")
                        {
                            dto.Id = new Guid(Id);
                            var result = await client.PutItem(dto.Id, dto);

                            if (result.Id != Guid.Empty)
                            {
                                DependencyService.Get<IToast>().Show("Edit event successfully");
                                await _navigation.PopModalAsync();
                            }
                        }
                        else
                        {
                            var result = await client.PostItem(dto);

                            if (result.Id != Guid.Empty)
                            {
                                DependencyService.Get<IToast>().Show("Event added successfully.");
                                await _navigation.PopModalAsync();
                            }
                        }

                        UserDialogs.Instance.HideLoading();

                    }
                    catch (Exception ex)
                    {
                        // TODO Add logger
                        UserDialogs.Instance.HideLoading();
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        private TimeSpan startTime;
        public TimeSpan StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                NotifyPropertyChanged("StartTime");
            }
        }
        private TimeSpan endTime;
        public TimeSpan EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                NotifyPropertyChanged();
            }
        }
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime SelectedDateTime;
        public DateTime _selectedDateTime
        {
            get
            {
                return SelectedDateTime;
            }
            set
            {
                SelectedDateTime = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime endDateTime;
        public DateTime EndDateTime
        {
            get
            {
                return endDateTime;
            }
            set
            {
                endDateTime = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<string> choiceList = new ObservableCollection<string>();
        public ObservableCollection<string> ChoiceList
        {
            get
            {
                return choiceList;
            }
            set
            {
                choiceList = value;
                NotifyPropertyChanged();
            }
        }

        private string selectedchoice;
        public string Selectedchoice
        {
            get
            {
                return selectedchoice;
            }
            set
            {
                selectedchoice = value;

                NotifyPropertyChanged();

            }
        }

        private ObservableCollection<string> dayList = new ObservableCollection<string>();
        public ObservableCollection<string> DayList
        {
            get
            {
                return dayList;
            }
            set
            {
                dayList = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime _MinDate;
        public DateTime MinDate
        {
            get
            {
                return _MinDate;
            }

            set
            {
                _MinDate = value;
                NotifyPropertyChanged();
            }
        }


        public NewEventViewModel(DateTime selectedDateTime, INavigation navigation)
        {

            try
            {
                _navigation = navigation;
                _selectedDateTime = selectedDateTime;
                EndDateTime = selectedDateTime;
                MinDate = selectedDateTime;
                adddData();
            }
            catch (Exception ex)
            {

            }

        }

        private void adddData()
        {
            try
            {
                _clientFactory = AppContainer.Container.Resolve<IClientFactory>();
                AddDataAsync().Wait(5);

                DayList.Add("Never");
                DayList.Add("Every Day");
                DayList.Add("Every Week");
                DayList.Add("Every 2 Weeks");
                DayList.Add("Every Month");
                DayList.Add("Every Year");

                //choices
                ChoiceList.Add("None");
                ChoiceList.Add("At time of event");
                ChoiceList.Add("5 minutes before");
                ChoiceList.Add("15 minutes before");
                ChoiceList.Add("30 minutes before");
                ChoiceList.Add("1 hour before");
                ChoiceList.Add("2 hour before");
                ChoiceList.Add("1 day before");
                ChoiceList.Add("2 days before");
                ChoiceList.Add("1 week before");


            }
            catch (Exception ex)
            {

            }
        }

        private string _selecteday;
        public string Selectedday
        {
            get
            {
                return _selecteday;
            }
            set
            {
                _selecteday = value;

                NotifyPropertyChanged();
                Xday = Selectedday;
            }
        }

        private string _Xday;
        public string Xday
        {
            get
            {
                return _Xday;
            }
            set
            {
                var val = value + " days";
                _Xday = val;
                NotifyPropertyChanged();
            }
        }


        public NewEventViewModel(CalandarAgendaModel obj, INavigation navigation)
        {
            try
            {
                _navigation = navigation;

                _selectedDateTime = obj.StartTime.ParseMyFormatDateTime();
                MinDate = _selectedDateTime;
                StartTime = _selectedDateTime.TimeOfDay;
                EndTime = obj.EndTime.ParseMyFormatDateTime().TimeOfDay;

                //StartTime = Convert.ToDateTime(obj.StartTime).TimeOfDay; 
                //EndTime= Convert.ToDateTime(obj.EndTime).TimeOfDay;

                Title = obj.Subject;
                Id = obj.id;
                Kategorieauswahlen = obj.icon;
                adddData();
                EditdataInfo(obj.id);
            }
            catch (Exception ex)
            {

            }

        }
        List<SupervisorDto> memberList = new List<SupervisorDto>();
        private async void EditdataInfo(string id)
        {
            try
            {
                CalendarEventDto client = await _clientFactory.CalendarEventClient.GetCalendarEventById(new Guid(id));
                if (client != null)
                {
                    MinDate = client.StartDate.ParseMyFormatDateTime(); ;
                    StartTime = MinDate.TimeOfDay;
                    EndTime = client.EndDate.ParseMyFormatDateTime().TimeOfDay;
                    Selectedchoice = client.PeriodOfTime;
                    Selectedday = client.EventSchedule;
                    memberList = client.Supervisors;
                    Kategorieauswahlen = client.Icon;

                    var e = client.EndDate.ParseMyFormatDateTime();
                    var m = e.Month;
                    EndDateTime = client.EndDate.ParseMyFormatDateTime();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async System.Threading.Tasks.Task AddDataAsync()
        {
            try
            {
                Usercollection.Clear();
                var Participans = await _clientFactory.SupervisorClient.GetGroupParticipanById(new Guid(Application.Current.Properties["supervisor_id"].ToString()));
                if (Participans != null)
                {
                    foreach (var item in Participans)
                    {
                        Usercollection.Add(new MemberModel()
                        {
                            Name = item.FirstName + " " + item.SecondName,
                            Id = item.Id.ToString(),
                            IsModerator = item.IsModerator,
                            IsSenior = item.IsSenior,
                            Email = item.EMail,
                            Type = item.IsModerator ? "Administrator" : "Unterkonto"
                        });
                    }
                }

            }
            catch (Exception ex)
            {

            }


        }


        ObservableCollection<MemberModel> usercollection = new ObservableCollection<MemberModel>();
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