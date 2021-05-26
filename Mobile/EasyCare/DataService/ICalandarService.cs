using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncfusion.SfCalendar.XForms;

namespace EasyCare.DataService
{
    public interface ICalandarService
    {
        void CreateService();
        Task<bool> CreateEvent(CalendarEventCollection eventsList);
    }
}
