using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using EasyCare.Core.Constants;
using EasyCare.ViewModels.Dashboard.Calendar;
using EasyCare.Views.Dashboard.Calendar;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;
namespace EasyCare.Behaviors.Calendar
{
    public class CalendarContentPageBehavior : Behavior<ContentPage>
    {
        private SfCalendar calendar;
        ListView listView;
        private CalendarEventCollection calendarInlineEvents;
        private ObservableCollection<string> meetingSubjectCollection;
        private ObservableCollection<Color> colorCollection;


        protected override void OnAttachedTo(ContentPage bindable)
        {

            var page = bindable as Page;
            calendar = page.FindByName<SfCalendar>("calendar");
            listView = page.FindByName<ListView>("ListView");
            calendar.OnInlineLoaded += Calendar_OnInlineLoaded;
         
        }

  

        private void Calendar_OnInlineLoaded(object sender, InlineEventArgs args)
        {
            try
            {
                CalandarAgendaGridViewModel viewModel = new CalandarAgendaGridViewModel();

                for (int i = 0; i < args.Appointments.Count; i++)
                {
                    string id = "";
                    string icon = IconsConstant.Drugs;
                    string imagepath="";
                    bool isAssignedSenior=false;

                    if (viewModel.Events != null && viewModel.Events.Count > 0)
                    {
                        int index = viewModel.Events.FindIndex(s => s.Id.ToString().ToLower().Equals(args.Appointments[i].AutomationId.ToString().ToLower()));

                        if (index != -1)
                        {
                            id = viewModel.Events[index].Id.ToString();
                            icon = IconsConstant.Health;
                          
                            if (viewModel.Events[index].Supervisors!=null && viewModel.Events[index].Supervisors.Count > 0 )
                            {
                                isAssignedSenior = true;
                                imagepath= GlobalConstant.Url + "/EasyCare/User/" + viewModel.Events[index].Supervisors[0].Id + ".jpg";
                            }
                            else
                            {
                                isAssignedSenior = false;
                            }

                            if (viewModel.Events[index].Icon != null && viewModel.Events[index].Icon != "")
                            {
                                if (viewModel.Events[index].Icon.Equals(IconsConstant.IconHealth))
                                {
                                    icon = IconsConstant.Health;
                                }
                                else if (viewModel.Events[index].Icon.Equals(IconsConstant.IconFreeTime))
                                {
                                    icon = IconsConstant.FreeTime;
                                }
                                else if (viewModel.Events[index].Icon.Equals(IconsConstant.IconDrugs))
                                {
                                    icon = IconsConstant.Drugs;
                                }
                                else if (viewModel.Events[index].Icon.Equals(IconsConstant.IconHouseholdWork))
                                {
                                    icon = IconsConstant.HouseHoldWork;
                                }
                                else if (viewModel.Events[index].Icon.Equals(IconsConstant.IconImportantEvent))
                                {
                                    icon = IconsConstant.ImportantEvent;
                                }
                                else if (viewModel.Events[index].Icon.Equals(IconsConstant.IconShopping))
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

                        }
                    }


                    viewModel.EventCollection.Add(new CalandarAgendaModel(isAssignedSenior,imagepath, id, i + 1, args.Appointments[i].Subject, args.Appointments[i].StartTime.ToString(), args.Appointments[i].EndTime.ToString(), icon));
                }

                listView.ItemsSource = viewModel.EventCollection;
                if (args.Appointments.Count != 0)
                {
                    args.View = new DataGrid() { BindingContext = viewModel };
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            calendar.OnInlineLoaded -= Calendar_OnInlineLoaded;
            base.OnDetachingFrom(bindable);
        }

        public CalendarContentPageBehavior()
        {

        }


    }
}
