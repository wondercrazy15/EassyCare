using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyCare.Core.Constants;
using EasyCare.Models.Settings;
using EasyCare.ViewModels.Dashboard.Calendar;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace EasyCare.Views.Dashboard.Calendar
{
    public partial class NewEvent : ContentPage
    {
        NewEventViewModel viewModels;
       
        public NewEvent(NewEventViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            viewModels = viewModel;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<App,ObservableCollection<MemberModel>> ((App)Xamarin.Forms.Application.Current, "SelectedMemberPopup", (sender,obj) =>
            {
                viewModels.Usercollection = obj;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App>((App)Xamarin.Forms.Application.Current, "ClearForms");

        }

        async void KategorieRight_Clicked(System.Object sender, System.EventArgs e)
        {
       
               string action = await DisplayActionSheet(null, "Abbrechen", null,
                    "Freizeit", "Einkaufen", "Medikamente", "Arzttermine", "Haushaltsarbeit", "Wichtiger Termin");
                if (action.Equals("Freizeit"))
                    viewModels.Kategorieauswahlen =  IconsConstant.IconFreeTime;
                else if (action.Equals("Einkaufen"))
                    viewModels.Kategorieauswahlen = IconsConstant.IconShopping;
                else if (action.Equals("Medikamente"))
                    viewModels.Kategorieauswahlen = IconsConstant.IconDrugs;
                else if (action.Equals("Arzttermine"))
                    viewModels.Kategorieauswahlen = IconsConstant.IconHealth;
                else if (action.Equals("Haushaltsarbeit"))
                    viewModels.Kategorieauswahlen = IconsConstant.IconHouseholdWork;
                else if (action.Equals("Wichtiger Termin"))
                    viewModels.Kategorieauswahlen = IconsConstant.IconImportantEvent;

        }
        //Member list popoup
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new MemberListPopup());
            }
            catch (Exception ex)
            {

            }
        }
        //Days popup
        async void Xdays_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                Xdays.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        void ErinnerungRight_Clicked(System.Object sender, System.EventArgs e)
        {
            choice.Focus();
        }

        void EndTime_Tapped(System.Object sender, System.EventArgs e)
        {
            EndTimePicker.Focus();
        }

        void StartTime_Tapped(System.Object sender, System.EventArgs e)
        {
            StartTimePicker.Focus();
        }
    }
}
