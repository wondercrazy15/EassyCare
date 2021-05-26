using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetChatMessages(Guid authorId, Guid receiverId);
    }
    
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Message>> GetChatMessages(Guid authorId, Guid receiverId)
        {
            return await _set.Where(x => x.AuthorId == authorId && x.ReceiverId == receiverId).ToListAsync();
        }
    }
}