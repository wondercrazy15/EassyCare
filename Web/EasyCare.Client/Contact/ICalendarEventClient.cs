using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface ICalendarEventClient : IBaseCrudClient<CalendarEventDto>
    {
        Task<IEnumerable<CalendarEventDto>> GetBySupervisorId(Guid supervisorId);
        Task<IEnumerable<CalendarEventDto>> GetEventBySupervisorId(Guid supervisorId);
        Task<IEnumerable<CalendarEventDto>> GetEventBySupervisorIdAndDate(Guid supervisorId, string Date);
        Task<IEnumerable<CalendarEventDateDto>> GetEventDateBySupervisorIdAndMonth(Guid supervisorId, int Month, int Year);
        Task<CalendarEventDto> GetCalendarEventById(Guid Id);
    }
}