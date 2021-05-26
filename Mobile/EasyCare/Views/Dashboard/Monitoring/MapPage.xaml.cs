using System;
using System.Collections.Generic;
using EasyCare.Core.Dto;
using EasyCare.Models;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Forms;

// TODO: Registration for Google Maps API Key
namespace EasyCare.Views.Dashboard.Monitoring
{
    public partial class MapPage : ContentPage
    {
        public MapPage(SeniorDto senior, double latitude, double longitude, DateTime date)
        {
            InitializeComponent();
            BindingContext = new MapViewModel(senior, longitude, latitude, date);
        }
    }
}
