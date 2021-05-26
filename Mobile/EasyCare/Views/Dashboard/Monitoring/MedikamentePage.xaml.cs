using System;
using System.Collections.Generic;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class MedikamentePage : ContentPage
    {
        private MedikamenteViewModel vm;

        public MedikamentePage()
        {
            vm = new MedikamenteViewModel(Navigation);
            InitializeComponent();
            BindingContext = vm;
          
            List<string> l = new List<string> {"hi" };
            DrugList.ItemsSource = l;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DrugList.ItemsSource = vm.LstDrugs;
            vm.GetDrugs();
        }

        async void AddNewMedikamente_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddNewMedikamentePage());
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<byte[]>(this, "ImageSelected");
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Event click
               // Grid grid = sender as Grid;
               // CalandarAgendaModel obj = grid.BindingContext as CalandarAgendaModel;

                //var viewModel = new NewEventViewModel(obj, Navigation);
               // await Navigation.PushModalAsync(new NewEvent(viewModel));
            }
            catch (Exception ex)
            {

            }
        }
    }
}
