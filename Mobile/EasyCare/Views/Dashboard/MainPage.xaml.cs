using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCare.Core.Dto;
using EasyCare.Models;
using EasyCare.ViewModels.Dashboard;
using EasyCare.ViewModels.Dashboard.Monitoring;
using EasyCare.Views.Dashboard.Calendar;
using EasyCare.Views.Dashboard.Chat;
using EasyCare.Views.Dashboard.Monitoring;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyCare.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage(SeniorDto senior = null, SupervisorDto supervisor = null, Guid? deviceId = null)
        {
            try
            {
                InitializeComponent();


                CurrentPage = Children[1];
                //if (senior != null && supervisor != null && deviceId.HasValue)
                //{
                    BindingContext = new MainPageViewModel(senior, supervisor, deviceId.Value, Navigation);

                    foreach (var page in Navigation.ModalStack)
                    {
                        Navigation.RemovePage(page);
                    }
                //}
            }
            catch (Exception ex)
            {

            }
          
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
