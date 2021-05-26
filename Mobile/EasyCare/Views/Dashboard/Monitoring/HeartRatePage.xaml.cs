using System;
using System.Collections.Generic;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class HeartRatePage : ContentPage
    {
        public HeartRatePage()
        {
            InitializeComponent();
            BindingContext = new HeartRateViewModel();
        }
    }
}
