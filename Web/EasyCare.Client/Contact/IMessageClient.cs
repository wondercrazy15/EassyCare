using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface IMessageClient  : IBaseCrudClient<MessageDto>
    {
        Task<IEnumerable<MessageDto>> GetChatMessages(Guid supervisorId, Guid receiverId);
    }
}