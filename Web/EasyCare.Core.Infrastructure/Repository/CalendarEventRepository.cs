using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ICalendarEventRepository : IRepository<CalendarEvent>
    {
        Task<IEnumerable<CalendarEvent>> GetBySupervisorId(Guid supervisorId);
        Task<IEnumerable<CalendarEvent>> GetEventBySupervisorId(Guid supervisorId);
        Task<IEnumerable<CalendarEvent>> GetEventBySupervisorIdAndDate(Guid supervisorId, string Date);
        Task<IEnumerable<CustomCalendarEventDate>> GetEventDateBySupervisorIdAndMonth(Guid supervisorId, int Month, int Year);
        Task<CustomCalendarEventDate> GetValidEventScheduleDate(string PeriodOfTime, DateTime StartDate, DateTime EndDate, DateTime CurrentDate);


    }

    public class CalendarEventRepository : BaseRepository<CalendarEvent>, ICalendarEventRepository
    {
        public CalendarEventRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CalendarEvent>> GetBySupervisorId(Guid supervisorId)
        {
            return await _context.CalendarEvent.Where(x => x.SupervisorId == supervisorId).ToListAsync();
        }

        public async Task<IEnumerable<CalendarEvent>> GetEventBySupervisorId(Guid supervisorId)
        {
            try
            {
                var calendarEvents = await _context.Set<CalendarEvent>().FromSqlRaw("EXECUTE dbo.GetEventBySupervisorId @p0", supervisorId).ToListAsync();
                return calendarEvents;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CalendarEvent>> GetEventBySupervisorIdAndDate(Guid supervisorId, string Date)
        {
            try
            {
                DateTime eventDate = Convert.ToDateTime(Date);
                var calendarEvents = await _context.Set<CalendarEvent>().FromSqlRaw("EXECUTE dbo.GetEventBySupervisorIdAndDate @p0, @p1", supervisorId, eventDate).ToListAsync();
               

                return calendarEvents;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CustomCalendarEventDate>> GetEventDateBySupervisorIdAndMonth(Guid supervisorId, int Month , int Year )
        {
            try
            {
                var calenderDate = await _context.Set<CustomCalendarEventDate>().FromSqlRaw("EXECUTE dbo.GetEventDateBySupervisorIdAndMonth @p0,@p1,@p2", supervisorId,Month,Year).ToListAsync();
                return calenderDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CustomCalendarEventDate> GetValidEventScheduleDate(string PeriodOfTime,DateTime StartDate,DateTime EndDate,DateTime CurrentDate)
        {
            try
            {
                SqlParameter param1 = new SqlParameter("@p0", PeriodOfTime);
                SqlParameter param2 = new SqlParameter("@p1", StartDate);
                SqlParameter param3 = new SqlParameter("@p3", EndDate);
                SqlParameter param4 = new SqlParameter("@p4", CurrentDate);
                var eventScheduleDate = await _context.Set<CustomCalendarEventDate>().FromSqlRaw("SELECT dbo.fnGetValidEventScheduleDate(@p0,@p1,@p3,@p4) as [Date]", param1,param2,param3,param4).FirstOrDefaultAsync();
                return eventScheduleDate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}