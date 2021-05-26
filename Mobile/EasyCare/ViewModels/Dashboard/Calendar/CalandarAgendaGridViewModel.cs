using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using EasyCare.DI;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Calendar
{
    public class CalandarAgendaGridViewModel
    {
        private ObservableCollection<CalandarAgendaModel> eventCollection = new ObservableCollection<CalandarAgendaModel>();
        public static List<CalendarEventDto> events { get; set; }
        public CalandarAgendaGridViewModel()
        {
        }


        public List<CalendarEventDto> Events
        {
            get { return events; }
        }

        /// <summary>
        /// Gets or sets the event collection.
        /// </summary>
        /// <value>The event collection.</value>
        public ObservableCollection<CalandarAgendaModel> EventCollection
        {
            get { return eventCollection; }
            set { this.eventCollection = value; }
        }
    }
}
