using System;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ISensorMessageRepository : IRepository<SensorMessage>
    {
        Task<SensorMessage> GetLastByDeviceId(Guid id);
    }
    
    public class SensorMessageRepository : BaseRepository<SensorMessage>, ISensorMessageRepository
    {
        public SensorMessageRepository(DatabaseContext context) : base(context)
        {
        }
        
        public async Task<SensorMessage> GetLastByDeviceId(Guid id)
        {
            var result = await _context.SensorMessage.Where((x => x.DeviceId == id)).ToListAsync();
            return result.Any() ? result.LastOrDefault() : null;
        }
    }
}