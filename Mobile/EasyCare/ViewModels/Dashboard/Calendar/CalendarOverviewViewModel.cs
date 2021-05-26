using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.DI;
using EasyCare.Views.Dashboard.Calendar;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Calendar
{
    public class CalendarOverviewViewModel : BaseViewModel
    {
        private INavigation _navigation;
        private ICalendarEventClient _client;

        public CalendarEventCollection Events { get; set; }
        private ObservableCollection<CalandarAgendaModel> eventCollection = new ObservableCollection<CalandarAgendaModel>();
        public DateTime SelectedDate { get; set; }

        public CalendarOverviewViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _client = AppContainer.Container.Resolve<IClientFactory>().CalendarEventClient;

            Events = new CalendarEventCollection();
            SelectedDate = DateTime.Now;
        }

        public ObservableCollection<CalandarAgendaModel> EventCollection
        {
            get { return eventCollection; }
            set { this.eventCollection = value; }
        }

        public async Task Refresh()
        {
            try
            {
                var supervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                var eventsForSupervisior = await _client.GetEventBySupervisorId(supervisorId);
                if (eventsForSupervisior != null)
                {
                    Events.Clear();
                    CalandarAgendaGridViewModel.events = new System.Collections.Generic.List<Core.Dto.CalendarEventDto>();

                    foreach (var @event in eventsForSupervisior)
                    {
                        var calendarEvent = new CalendarInlineEvent
                        {
                            AutomationId=@event.Id.ToString(),
                            Color=Color.Red,
                            StartTime = DateTime.Parse(@event.StartDate).ToLocalTime(),
                            EndTime = DateTime.Parse(@event.EndDate).ToLocalTime(),
                            Subject = @event.Title
                        };
                        CalandarAgendaGridViewModel.events.Add(@event);
                        Events.Add(calendarEvent);
                    }
                   
                }
            }
            catch (Exception ex)
            {

            }
            
        }

        internal async Task GetSelectedEventMonth(int selectedMonth,int selectedYear)
        {
            try
            {
                EventCollection.Clear();
                var supervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                var eventsForSupervisior = await _client.GetEventDateBySupervisorIdAndMonth(supervisorId, selectedMonth, selectedYear);
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
                        //CalandarAgendaGridViewModel.events.Add(@event);
                        EventCollection.Clear();
                        Events.Add(calendarEvent);
                        
                    }

                }
               
            }
            catch (Exception ex)
            {

            }
        }

        private bool _isBackgrounVisible;
        public bool IsBackgrounVisible
        {
            get
            {
                return _isBackgrounVisible;
            }
            set
            {
                _isBackgrounVisible = value;
                NotifyPropertyChanged("IsBackgrounVisible");
            }
        }

        internal async Task GetSelectedEvent(DateTime? selectedDate)
        {
            try
            {
                
                var supervisorId = (Guid)Application.Current.Properties["supervisor_id"];
                var eventsForSupervisior = await _client.GetEventBySupervisorIdAndDate(supervisorId,selectedDate.Value.ToString("yyyy-MM-dd"));
                EventCollection.Clear();
                if (eventsForSupervisior != null)
                {
                    for (int i = 0; i < eventsForSupervisior.ToList().Count; i++)
                    {
                            var item = eventsForSupervisior.ToList()[i];
                            string id = "";
                            string icon = IconsConstant.Drugs;
                            string imagepath = "";
                            bool isAssignedSenior = false;

                       
                            id = item.Id.ToString();
                            icon = IconsConstant.Health;

                            if (item.Supervisors != null && item.Supervisors.Count > 0)
                            {
                                isAssignedSenior = true;
                            IsBackgrounVisible = true;
                                imagepath = GlobalConstant.Url + "/EasyCare/User/" + item.Supervisors[0].Id + ".jpg";
                            }
                            else
                            {
                                isAssignedSenior = false;
                            IsBackgrounVisible = false;
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

                        EventCollection.Add(new CalandarAgendaModel(isAssignedSenior, imagepath, id, i + 1,item.Title, item.StartDate.ToString(), item.EndDate.ToString(), icon, item.Icon));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public Command EventSelectCommand
        {
            get
            {
                return new Command(async (Object obj) =>
                {
                    var viewModel = new NewEventViewModel(SelectedDate, _navigation);
                    await _navigation.PushModalAsync(new NewEvent(viewModel));
                });
            }
        }
        public Command NavigateToAddEventsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var viewModel = new NewEventViewModel(SelectedDate, _navigation);
                   
                    await _navigation.PushModalAsync(new NewEvent(viewModel));
                });
            }
        }
    }
}

