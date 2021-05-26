using EasyCare.Core.Constants;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ICalendarEventSchedulersRepository : IRepository<CalendarEventSchedulers>
    {
        //Can extends for custom method
        Task<bool> UpdateCalendarEventSchedulers(CustomCalendarEventSchedulers item);
        Task<IEnumerable<CustomEventScheduler>> GetEventScheduler();
        Task<CalendarEventSchedulers> AddCalendarEventSchedulers(CustomCalendarEventSchedulers item);
        Task<CalendarEventSchedulers> GetEventSchedulersByEventId(Guid eventId);
    }

    public class CalendarEventSchedulersRepository : BaseRepository<CalendarEventSchedulers>, ICalendarEventSchedulersRepository
    {
        DatabaseContext _context = null;
        public CalendarEventSchedulersRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CalendarEventSchedulers> AddCalendarEventSchedulers(CustomCalendarEventSchedulers item)
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-US");

                var eventDate = item.StartDate.ToString("MM/dd/yyyy", culture);
                var eventTime = item.StartDate.ToString("HH:mm:ss");
                var weekDay = item.StartDate.ToString("dddd");
                var monthDate = item.StartDate.ToString("dd");
                var month = item.StartDate.ToString("MMMM");

                CalendarEventSchedulers objCalendarEventSchedulers = new CalendarEventSchedulers();

                objCalendarEventSchedulers.CalenderEventId = item.Id;
                objCalendarEventSchedulers.PeriodOfTime = item.PeriodOfTime;
                objCalendarEventSchedulers.CreatedDate = DateTime.Now;

                if (item.Days >= 0)
                {
                    objCalendarEventSchedulers.Days = item.Days;
                }
                if (item.EventSchedule == EventScheduleconsConstant.Never)
                {
                    objCalendarEventSchedulers.OneTimeDate = item.StartDate;
                }
                else if (item.EventSchedule == EventScheduleconsConstant.EveryDay)
                {
                    objCalendarEventSchedulers.IsDaily = true;
                    objCalendarEventSchedulers.Time = TimeSpan.Parse(eventTime);
                }
                else if (item.EventSchedule == EventScheduleconsConstant.EveryWeek)
                {
                    objCalendarEventSchedulers.IsWeekly = true;
                    objCalendarEventSchedulers.WeekDay = weekDay;
                    objCalendarEventSchedulers.Time = TimeSpan.Parse(eventTime);
                }
                else if (item.EventSchedule == EventScheduleconsConstant.Every2Weeks)
                {
                    objCalendarEventSchedulers.Is2Weekly = true;
                    objCalendarEventSchedulers.WeekDay = weekDay;
                    objCalendarEventSchedulers.Time = TimeSpan.Parse(eventTime);
                }
                else if (item.EventSchedule == EventScheduleconsConstant.EveryMonth)
                {
                    objCalendarEventSchedulers.IsMonthly = true;
                    objCalendarEventSchedulers.MonthDate = Convert.ToInt32(monthDate);
                    objCalendarEventSchedulers.Time = TimeSpan.Parse(eventTime);
                }
                else if (item.EventSchedule == EventScheduleconsConstant.EveryYear)
                {
                    objCalendarEventSchedulers.IsYearly = true;
                    objCalendarEventSchedulers.Month = month;
                    objCalendarEventSchedulers.MonthDate = Convert.ToInt32(monthDate);
                    objCalendarEventSchedulers.Time = TimeSpan.Parse(eventTime);
                }

                await _set.AddAsync(objCalendarEventSchedulers);
                await _context.SaveChangesAsync();

                return objCalendarEventSchedulers;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCalendarEventSchedulers(CustomCalendarEventSchedulers item)
        {
            try
            {
                //var eventDate = item.StartDate.ToString("yyyy-MM-dd"); ;
                CultureInfo culture = new CultureInfo("en-US");

                var eventDate = item.StartDate.ToString("MM/dd/yyyy", culture);
                var eventTime = item.StartDate.ToString("HH:mm:ss");
                var weekDay = item.StartDate.ToString("dddd");
                var monthDate = item.StartDate.ToString("dd");
                var month = item.StartDate.ToString("MMMM");

                var entity = _set.Where(x => x.CalenderEventId == item.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity.PeriodOfTime = item.PeriodOfTime;
                    entity.ModifiedDate = DateTime.Now;

                    if (item.EventSchedule == EventScheduleconsConstant.Never)
                    {
                        entity.OneTimeDate = item.StartDate;
                    }
                    else if (item.EventSchedule == EventScheduleconsConstant.EveryDay)
                    {
                        entity.IsDaily = true;
                        entity.Time = TimeSpan.Parse(eventTime);
                    }
                    else if (item.EventSchedule == EventScheduleconsConstant.EveryWeek)
                    {
                        entity.IsWeekly = true;
                        entity.WeekDay = weekDay;
                        entity.Time = TimeSpan.Parse(eventTime);
                    }
                    else if (item.EventSchedule == EventScheduleconsConstant.Every2Weeks)
                    {
                        entity.Is2Weekly = true;
                        entity.WeekDay = weekDay;
                        entity.Time = TimeSpan.Parse(eventTime);
                    }
                    else if (item.EventSchedule == EventScheduleconsConstant.EveryMonth)
                    {
                        entity.IsMonthly = true;
                        entity.MonthDate = Convert.ToInt32(monthDate);
                        entity.Time = TimeSpan.Parse(eventTime);
                    }
                    else if (item.EventSchedule == EventScheduleconsConstant.EveryYear)
                    {
                        entity.IsYearly = true;
                        entity.Month = month;
                        entity.MonthDate = Convert.ToInt32(monthDate);
                        entity.Time = TimeSpan.Parse(eventTime);
                    }
                    _set.Update(entity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async Task<IEnumerable<CustomEventScheduler>> GetEventScheduler()
        {
            try
            {
                var eventScheduler = await _context.Set<CustomEventScheduler>().FromSqlRaw("EXECUTE dbo.GetEventScheduler").ToListAsync();
                return eventScheduler;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CalendarEventSchedulers> GetEventSchedulersByEventId(Guid eventId)
        {
            try
            {
                CalendarEventSchedulers eventSchedulerEntity = null;

                eventSchedulerEntity = await _set.Where(x => x.CalenderEventId == eventId).FirstOrDefaultAsync();

                return eventSchedulerEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
