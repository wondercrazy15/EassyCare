using System;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class CalendarEvent : BaseEntity
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SupervisorId { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
    }

    public class CustomCalendarEventDate
    {
        public DateTime Date { get; set; }
    }
}