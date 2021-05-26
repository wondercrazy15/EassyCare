using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<IEnumerable<Device>> GetBySupervisorId(Guid supervisorId);
        Task<Device> AddDevice(Guid SupervisorId);
       // Task<TAN> GetDeviceByEmailAndCode(string Code, string EmailId);
    }
    
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Device>> GetBySupervisorId(Guid supervisorId)
        {
            try
            {
                return await _set.Where(x => x.SupervisorId == supervisorId).ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Device> AddDevice(Guid SupervisorId)
        {
            Device entity = new Device();
            entity.SupervisorId = SupervisorId;
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        //public async Task<TAN> GetDeviceByEmailAndCode(string Code, string EmailId)
        //{
        //    TAN objDevice = new TAN();

        //    if(!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(EmailId))
        //    {
        //        objDevice = await (from d in _context.Device
        //                 join s in _context.Supervisor on d.SupervisorId equals s.Id
        //                 join t in _context.TAN on d.Id equals t.DeviceId
        //                 where s.EMail == EmailId && t.Code == Code
        //                 select t).FirstOrDefaultAsync();
        //    }
             

        //    return objDevice;
        //}
    }
}