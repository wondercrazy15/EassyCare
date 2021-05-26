using System;
using System.Collections.Generic;

namespace EasyCare.Core.Dto
{
    public class CalendarEventDto
    {
        public string Icon { get; set; }

        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        
        public Guid SupervisorId { get; set; }

        public string[] CalenderSupervisorId { get; set; } = Array.Empty<string>();

        public string PeriodOfTime { get; set; }

        public string EventSchedule { get; set; }
        public DateTime EventScheduleDate { get; set; }
        public List<SupervisorDto> Supervisors { get; set; }
    }

    public class CalendarEventDateDto
    {
        public DateTime Date { get; set; }
    }
}