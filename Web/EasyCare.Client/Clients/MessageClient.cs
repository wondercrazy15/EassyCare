using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class MessageClient : BaseCrudClient<MessageDto>, IMessageClient
    {
        public MessageClient(Options options) : base(options, "Message")
        {
        }

        public async Task<IEnumerable<MessageDto>> GetChatMessages(Guid supervisorId, Guid receiverId)
        {
            return await Get<IEnumerable<MessageDto>>($"chat/{supervisorId}/{receiverId}");
        }
    }
}