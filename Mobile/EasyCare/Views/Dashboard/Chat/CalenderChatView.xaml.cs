using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.ViewModels.Dashboard.Calendar;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class CalenderChatView : CustomGrid
    {
        public CalendarEventCollection Events { get; set; }
        private ICalendarEventClient _clients;
        int Selectedmonth = 0;
        int SelectedYear = 0;
        public CalenderChatView()
        {
            InitializeComponent();
            calendar.NavigationDirection = NavigationDirection.Horizontal;
            calendar.Forward();
            calendar.Backward();
            Events = new CalendarEventCollection();
            _clients = AppContainer.Container.Resolve<IClientFactory>().CalendarEventClient;
           
            Month.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(calendar.MoveToDate.Month);
            Year.Text = calendar.MoveToDate.Year.ToString();
            calendar.MonthChanged += Calendar_MonthChanged;
        }
        void Left_Clicked(object sender, System.EventArgs e)
        {
            calendar.Backward();
        }
        void Right_Clicked(object sender, System.EventArgs e)
        {
            calendar.Forward();
        }
        void Calendar_MonthChanged(object sender, MonthChangedEventArgs args)
        {
            Month.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(args.CurrentValue.Month);
            Year.Text = args.CurrentValue.Year.ToString();
            Selectedmonth = args.CurrentValue.Month;
            SelectedYear = args.CurrentValue.Year;
            CalenderDataBind(Selectedmonth, SelectedYear);
        }
        async void CalenderDataBind(int selectedMonth, int selectedYear)
        {
            try
            {
                var supervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                var eventsForSupervisior = await _clients.GetEventDateBySupervisorIdAndMonth(supervisorId, selectedMonth, selectedYear);
                if (eventsForSupervisior != null)
                {
                    Events.Clear();
                    CalandarAgendaGridViewModel.events = new System.Collections.Generic.List<Core.Dto.CalendarEventDto>();

                    foreach (var @event in eventsForSupervisior)
                    {
                        var calendarEvent = new CalendarInlineEvent
                        {

                            Color = Color.Red,
                            StartTime = @event.Date.ToLocalTime(),
                            EndTime = @event.Date.ToLocalTime(),

                        };
                        Events.Add(calendarEvent);

                    }
                    calendar.DataSource = Events;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private ObservableCollection<CalandarAgendaModel> eventCollection = new ObservableCollection<CalandarAgendaModel>();
        private ITwilioChatHelper twilioChatHelper;

        public ObservableCollection<CalandarAgendaModel> EventCollection
        {
            get { return eventCollection; }
            set { this.eventCollection = value; }
        }
        internal async Task GetSelectedEvent(DateTime? selectedDate)
        {
            try
            {

                var supervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                var eventsForSupervisior = await _clients.GetEventBySupervisorIdAndDate(supervisorId, selectedDate.Value.ToString("yyyy-MM-dd"));
                EventCollection.Clear();
                if (eventsForSupervisior != null)
                {
                    if (eventsForSupervisior.ToList().Count > 0)
                    {
                        calendar.IsVisible = false;
                        EventList.IsVisible = true;


                        for (int i = 0; i < eventsForSupervisior.ToList().Count; i++)
                        {
                            var item = eventsForSupervisior.ToList()[i];
                            string id = "";
                            string iconName = item.Icon;
                            string icon = IconsConstant.Drugs;
                            string imagepath = "";
                            bool isAssignedSenior = false;


                            id = item.Id.ToString();
                            icon = IconsConstant.Health;

                            if (item.Supervisors != null && item.Supervisors.Count > 0)
                            {
                                isAssignedSenior = true;
                                imagepath = GlobalConstant.Url + "/EasyCare/User/" + item.Supervisors[0].Id + ".jpg";
                            }
                            else
                            {
                                isAssignedSenior = false;
                            }

                            if (item.Icon != null && item.Icon != "")
                            {
                                if (item.Icon.Equals(IconsConstant.IconHealth))
                                {
                                    icon = IconsConstant.Health;
                                }
                                else if (item.Icon.Equals(IconsConstant.IconFreeTime))
                                {
                                    icon = IconsConstant.FreeTime;
                                }
                                else if (item.Icon.Equals(IconsConstant.IconDrugs))
                                {
                                    icon = IconsConstant.Drugs;
                                }
                                else if (item.Icon.Equals(IconsConstant.IconHouseholdWork))
                                {
                                    icon = IconsConstant.HouseHoldWork;
                                }
                                else if (item.Icon.Equals(IconsConstant.IconImportantEvent))
                                {
                                    icon = IconsConstant.ImportantEvent;
                                }
                                else if (item.Icon.Equals(IconsConstant.IconShopping))
                                {
                                    icon = IconsConstant.Shopping;
                                }
                                else
                                {
                                    icon = IconsConstant.HouseHoldWork;
                                }
                            }
                            else
                            {
                                icon = IconsConstant.HouseHoldWork;
                            }

                            EventCollection.Add(new CalandarAgendaModel(isAssignedSenior, imagepath, id, i + 1, item.Title, item.StartDate.ToString(), item.EndDate.ToString(), icon,iconName));
                        }
                        ListView.ItemsSource = EventCollection;
                    }
                    else
                    {
                        DependencyService.Get<IToast>().Show("There is no event");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void calendar_OnCalendarTapped(System.Object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs e)
        {
            var data = sender as SfCalendar;
            var Sdate = data.SelectedDate;
            
            await GetSelectedEvent(Sdate);
        }

        void Back_Clicked(System.Object sender, System.EventArgs e)
        {
            calendar.IsVisible = true;
            EventList.IsVisible = false;
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var item = (CalandarAgendaModel)e.SelectedItem;
            if (item != null)
            {
                twilioChatHelper = DependencyService.Get<ITwilioChatHelper>();
                twilioChatHelper.SendCalenderEvent(item, true, Application.Current.Properties["supervisor_id"].ToString(), this.ViewModel.GroupKey);
                this.ViewModel.attachmentPopup.Dismiss();

            }
        }
    }
}
