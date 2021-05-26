using EasyCare.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Domain.Entities
{
    public class CalendarEventSchedulers : BaseEntity
    {
        public Guid CalenderEventId { get; set; }
        public bool IsYearly { get; set; }
        public string Month { get; set; }
        public bool IsMonthly { get; set; }
        public Nullable<int> MonthDate { get; set; }
        public bool Is2Weekly { get; set; }
        public bool IsWeekly { get; set; }
        public string WeekDay { get; set; }
        public bool IsDaily { get; set; }
        public Nullable<TimeSpan> Time { get; set; }
        public Nullable<DateTime> OneTimeDate { get; set; }
        public string PeriodOfTime { get; set; }
        public int Days { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }

    public class CustomCalendarEventSchedulers
    {
        public Guid Id { get; set; }
        public string PeriodOfTime { get; set; }
        public string EventSchedule { get; set; }
        public DateTime StartDate { get; set; }
        public int Days { get; set; }
    }

    public class CustomEventScheduler
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Tags { get; set; }
        public Guid SupervisorId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
