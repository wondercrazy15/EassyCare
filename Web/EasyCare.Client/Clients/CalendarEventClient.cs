using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class CalendarEventClient : BaseCrudClient<CalendarEventDto>, ICalendarEventClient
    {
        public CalendarEventClient(Options options) : base(options, "CalendarEvent")
        {
        }

        public async Task<IEnumerable<CalendarEventDto>> GetBySupervisorId(Guid supervisorId)
        {
            return await Get<IEnumerable<CalendarEventDto>>($"supervisor/{supervisorId}");
        }

        public async Task<IEnumerable<CalendarEventDto>> GetEventBySupervisorId(Guid supervisorId)
        {
            return await Get<IEnumerable<CalendarEventDto>>($"GetEventBySupervisorId/{supervisorId}");
        }

        public async Task<IEnumerable<CalendarEventDto>> GetEventBySupervisorIdAndDate(Guid supervisorId, string Date)
        {
            return await Get<IEnumerable<CalendarEventDto>>($"GetEventBySupervisorIdAndDate/{supervisorId}/{Date}");
        }

        public async Task<IEnumerable<CalendarEventDateDto>> GetEventDateBySupervisorIdAndMonth(Guid supervisorId, int Month, int Year)
        {
            return await Get<IEnumerable<CalendarEventDateDto>>($"GetEventDateBySupervisorIdAndMonth/{supervisorId}/{Month}/{Year}");
        }

        public async Task<CalendarEventDto> GetCalendarEventById(Guid Id)
        {
            return await Get<CalendarEventDto>($"GetCalendarEventById/{Id}");
        }
    }
}