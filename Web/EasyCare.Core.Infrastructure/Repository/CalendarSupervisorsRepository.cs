using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Core.Infrastructure.Repository
{
    
    public interface ICalendarSupervisorsRepository : IRepository<CalendarSupervisors>
    {
        //Can extends for custom method
        Task<bool> AddOrUpdateCalendarSupervisors(Guid CalenderEventId, string[] CalendarSupervisorIds, DateTime InvitedDate);
    }

    public class CalendarSupervisorsRepository : BaseRepository<CalendarSupervisors>, ICalendarSupervisorsRepository
    {
        public CalendarSupervisorsRepository(DatabaseContext context) : base(context)
        {

        }

        public async Task<bool> AddOrUpdateCalendarSupervisors(Guid CalenderEventId, string[] CalendarSupervisorIds,DateTime InvitedDate)
        {
            if (CalendarSupervisorIds.Length > 0)
            {
                var entities = _set.Where(x => x.CalenderEventId == CalenderEventId).ToList();
                if (entities.Count() > 0)
                {
                    _set.RemoveRange(entities);
                    await _context.SaveChangesAsync();
                }
                foreach (var supervisorId in CalendarSupervisorIds)
                {
                    CalendarSupervisors calendarSupervisors = new CalendarSupervisors();
                    calendarSupervisors.CalenderEventId = CalenderEventId;
                    calendarSupervisors.SupervisorId =Guid.Parse(supervisorId);
                    calendarSupervisors.InvitedDate = InvitedDate;

                    _set.Add(calendarSupervisors);
                    await _context.SaveChangesAsync();
                  
                }
                return true;
            }
            return false;
        }
    }
}
