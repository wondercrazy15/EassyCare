using EasyCare.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Domain.Entities
{
    public class CalendarSupervisors : BaseEntity
    {
        public Guid CalenderEventId { get; set; }
        public Guid SupervisorId { get; set; }
        public DateTime InvitedDate { get; set; }
        public string Message { get; set; }
    }
}
