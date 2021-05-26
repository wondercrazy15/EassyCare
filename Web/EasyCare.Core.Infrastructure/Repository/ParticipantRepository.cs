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
    public interface IParticipantRepository : IRepository<Participant>
    {
        //Can extends for custom method
        Task<Participant> GetBySupervisorId(Guid supervisorId);
        Task<Participant> GetGroupParticipant(Guid participantId, Guid groupId);
        Task<Participant> AddParticipant(Guid SupervisorId, Guid GroupId);
    }
    public class ParticipantRepository: BaseRepository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(DatabaseContext context) : base(context)
        {

        }

        public async Task<Participant> GetBySupervisorId(Guid supervisorId)
        {
            try
            {
                return await _set.Where(x => x.ParticipationId == supervisorId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Participant> GetGroupParticipant(Guid participantId, Guid groupId)
        {
            try
            {
                return await _set.Where(x => x.ParticipationId == participantId && x.GroupId==groupId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<Participant> AddParticipant(Guid SupervisorId,Guid GroupId)
        {
            Participant entity = new Participant();
            entity.ParticipationId = SupervisorId;
            entity.GroupId = GroupId;
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
