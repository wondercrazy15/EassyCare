using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ITANRepository : IRepository<TAN>
    {
        Task<TAN> GetByCode(string code);
        Task<TAN> AddTAN(Guid DeviceId, string Code, string NotificationTagCode);
        Task<IEnumerable<CustomTAN>> GetTagsByEventId(Guid CalendarEventId);
        Task<IEnumerable<CustomTAN>> GetTagsByGroupId(Guid GroupId, Guid SupervisiorId);
        Task<TAN> GetByNotificationTagCode(string NotificationTagCode);
    }

    public class TANRepository : BaseRepository<TAN>, ITANRepository
    {
        DatabaseContext _context = null;
        public TANRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TAN> GetByCode(string code)
        {
            return await _context.TAN.Where(x => x.Code == code).FirstOrDefaultAsync();
        }

        public async Task<TAN> GetByNotificationTagCode(string NotificationTagCode)
        {
            return await _context.TAN.Where(x => x.NotificationTagCode == NotificationTagCode).FirstOrDefaultAsync();
        }

        public async Task<TAN> AddTAN(Guid DeviceId, string Code,string NotificationTagCode)
        {
            TAN entity = null;
            if (!string.IsNullOrEmpty(Code))
            {
                var TANExsit = await _set.Where(x => x.Code == Code).FirstOrDefaultAsync();
                if (TANExsit == null)
                {
                    entity = new TAN();
                    entity.DeviceId = DeviceId;
                    entity.Code = Code;
                    entity.NotificationTagCode = NotificationTagCode;
                    await _set.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }
            }
            return entity;
        }

        public async Task<IEnumerable<CustomTAN>> GetTagsByEventId(Guid CalendarEventId)
        {
            try
            {
                var objCalendarSupervisors = await (from cs in _context.CalendarSupervisors
                                                    join d in _context.Device on cs.SupervisorId equals d.SupervisorId
                                                    join t in _context.TAN on d.Id equals t.DeviceId
                                                    where cs.CalenderEventId == CalendarEventId
                                                    select new CustomTAN
                                                    {
                                                        Tags = t.NotificationTagCode
                                                    }).ToListAsync();

                return objCalendarSupervisors;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<CustomTAN>> GetTagsByGroupId(Guid GroupId, Guid SupervisiorId)
        {
            try
            {
                var objGroupSupervisors = await (from p in _context.Participant
                                                 join d in _context.Device on p.ParticipationId equals d.SupervisorId
                                                 join t in _context.TAN on d.Id equals t.DeviceId
                                                 where p.GroupId == GroupId && p.ParticipationId != SupervisiorId
                                                 select new CustomTAN
                                                 {
                                                     Tags = t.NotificationTagCode
                                                 }).ToListAsync();

                return objGroupSupervisors;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}