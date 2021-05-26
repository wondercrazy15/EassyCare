using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface INotificationMessageRepository : IRepository<NotificationMessage>
    {
        Task<IEnumerable<NotificationMessage>> GetNotificationMessage(Guid deviceId);
        Task ConfirmNotificationMessages(Guid deviceId);

    }

    public class NotificationMessageRepository : BaseRepository<NotificationMessage>, INotificationMessageRepository
    {
        public NotificationMessageRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NotificationMessage>> GetNotificationMessage(Guid deviceId)
        {
            return await _set.Where(x => x.DeviceId == deviceId).ToListAsync();
        }

        public async Task ConfirmNotificationMessages(Guid deviceId)
        {
            var toDeleteList = await _set.Where(x => x.DeviceId == deviceId).ToListAsync();
            _set.RemoveRange(toDeleteList);
            await _context.SaveChangesAsync();
        }
    }
}