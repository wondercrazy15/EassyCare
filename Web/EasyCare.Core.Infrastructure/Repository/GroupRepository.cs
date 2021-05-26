using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface IGroupRepository : IRepository<Group>
    {
        //Can extends for custom method
        Task<Group> GetGroupByCode(string Code);
        Task<Group> GetGroupBySupervisorId(Guid SupervisorId);
    }
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        DatabaseContext _context = null;
        public GroupRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Group> GetGroupByCode(string Code)
        {
            try
            {
                return await _set.Where(x => x.Code == Code).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Group> GetGroupBySupervisorId(Guid SupervisorId)
        {
            try
            {
                Group objGroup = null;
               
                    objGroup = await (from p in _context.Participant
                                join g in _context.Group on p.GroupId equals g.Id
                                where p.ParticipationId == SupervisorId
                                select g).FirstOrDefaultAsync();
                
                return objGroup;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }
}
