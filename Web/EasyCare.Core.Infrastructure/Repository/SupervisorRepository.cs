using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ISupervisorRepository : IRepository<Supervisor>
    {
        // Can extends for custom method
        Task<Supervisor> GetSupervisorByEmail(string Email);
        Task<IEnumerable<Supervisor>> GetGroupParticipanById(Guid supervisorId);
        Task<IEnumerable<Supervisor>> GetSupervisorByGroupId(Guid GroupId, bool IsModerator);
        Task<Supervisor> GetSeniorBySupervisorId(Guid SupervisorId);
        Task<IEnumerable<Supervisor>> GetCalendarSupervisorsByEventId(Guid CalendarEventId);
    }

    public class SupervisorRepository : BaseRepository<Supervisor>, ISupervisorRepository
    {
        DatabaseContext _context = null;
        public SupervisorRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }


        public async Task<Supervisor> GetSupervisorByEmail(string Email)
        {
            try
            {
                return await _set.Where(x => x.EMail == Email).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Supervisor>> GetGroupParticipanById(Guid supervisorId)
        {
            try
            {
                var participans = await _context.Set<Supervisor>().FromSqlRaw("EXECUTE dbo.GetGroupParticipanById @p0", supervisorId).ToListAsync();
                return participans;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Supervisor>> GetSupervisorByGroupId(Guid GroupId, bool IsModerator)
        {
            try
            {
                List<Supervisor> supervisorList = null;
                if (IsModerator)
                {
                    supervisorList = await (from p in _context.Participant
                                            join s in _context.Supervisor on p.ParticipationId equals s.Id
                                            where p.GroupId == GroupId && s.IsModerator == true
                                            select s).ToListAsync();
                }
                else
                {
                    supervisorList = await (from p in _context.Participant
                                            join s in _context.Supervisor on p.ParticipationId equals s.Id
                                            where p.GroupId == GroupId && s.IsSenior == true
                                            select s).ToListAsync();
                }

                return supervisorList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Supervisor> GetSeniorBySupervisorId(Guid SupervisorId)
        {
            try
            {
                var objSenior = await (from p in _context.Participant
                                       join g in _context.Group on p.GroupId equals g.Id
                                       join gp in _context.Participant on g.Id equals gp.GroupId
                                       join s in _context.Supervisor on gp.ParticipationId equals s.Id
                                       where p.ParticipationId == SupervisorId && s.IsSenior == true
                                       && s.IsActive == true
                                       select s).FirstOrDefaultAsync();

                return objSenior;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Supervisor>> GetCalendarSupervisorsByEventId(Guid CalendarEventId)
        {
            try
            {
                var objCalendarSupervisors = await (from cs in _context.CalendarSupervisors
                                       join s in _context.Supervisor on cs.SupervisorId equals s.Id
                                       where cs.CalenderEventId==CalendarEventId 
                                       && s.IsActive == true
                                       select s).ToListAsync();

                return objCalendarSupervisors;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}