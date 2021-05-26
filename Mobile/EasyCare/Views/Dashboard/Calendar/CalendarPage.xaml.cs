using System;
using System.Collections.Generic;
using EasyCare.DataService;
using EasyCare.ViewModels.Dashboard.Calendar;
using Syncfusion.SfCalendar.XForms;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Calendar
{
    public partial class CalendarPage : ContentPage
    {
        bool isweekly = false;
        SfPopupLayout popupLayout;
     
        List<string> menuList = new List<string> { };
        int Selectedmonth = 0;
        int SelectedYear = 0;

        public CalendarPage()
        {
            InitializeComponent();
            calendar.NavigationDirection = NavigationDirection.Horizontal;
            calendar.Forward();
            calendar.Backward();
            Month.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(calendar.MoveToDate.Month);
            Year.Text = calendar.MoveToDate.Year.ToString();
            calendar.MonthChanged += Calendar_MonthChanged;
            calendar.OnCalendarTapped += Calendar_OnCalendarTapped;
        }

        private async void Calendar_OnCalendarTapped(object sender, CalendarTappedEventArgs e)
        {
            await(BindingContext as CalendarOverviewViewModel).GetSelectedEvent(calendar.SelectedDate);
        }

        protected async override void OnAppearing()
        {
            try
            {
                await (BindingContext as CalendarOverviewViewModel).GetSelectedEventMonth(Selectedmonth, SelectedYear);
                await (BindingContext as CalendarOverviewViewModel).GetSelectedEvent(calendar.SelectedDate);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

       async  void Calendar_MonthChanged(object sender, MonthChangedEventArgs args)
        {
            Month.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(args.CurrentValue.Month);
            Year.Text = args.CurrentValue.Year.ToString();
            Selectedmonth = args.CurrentValue.Month;
            SelectedYear = args.CurrentValue.Year;
            await(BindingContext as CalendarOverviewViewModel).GetSelectedEventMonth(Selectedmonth, SelectedYear);
        }
       async void Left_Clicked(object sender, System.EventArgs e)
        {
            calendar.Backward();
            await(BindingContext as CalendarOverviewViewModel).GetSelectedEventMonth(Selectedmonth, SelectedYear);
        }
       async void Right_Clicked(object sender, System.EventArgs e)
        {
            calendar.Forward();
            await(BindingContext as CalendarOverviewViewModel).GetSelectedEventMonth(Selectedmonth, SelectedYear);
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            //Event click
            Grid grid = sender as Grid;
            CalandarAgendaModel obj = grid.BindingContext as CalandarAgendaModel;

            var viewModel = new NewEventViewModel(obj, Navigation);
            await Navigation.PushModalAsync(new NewEvent(viewModel));
        }

        async void Headermenu_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {

                string action = await DisplayActionSheet(null, "Abbrechen", null, "Mit lokalem Kalender synchronisieren");
                var c = action;
                if (action.Equals("Abbrechen"))
                {
                }
                else if (action.Equals("Mit lokalem Kalender synchronisieren"))
                {
                    bool ISAdded = await DependencyService.Get<ICalandarService>(DependencyFetchTarget.GlobalInstance).CreateEvent((BindingContext as CalendarOverviewViewModel).Events);
                    if (ISAdded)
                    {
                        //added successfully
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

    }
}
