using System;
using System.Collections.Generic;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class NotificationsPage : ContentPage
    {
        public NotificationsPage()
        {
            try
            {
                InitializeComponent();
               BindingContext = new NotificationsViewModel();
            }
            catch (Exception ex)
            {

            }
          
        }
    }
}
