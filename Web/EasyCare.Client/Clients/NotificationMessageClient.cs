using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class NotificationMessageClient : BaseCrudClient<NotificationMessageDto>, INotificationMessageClient
    {
        public NotificationMessageClient(Options options) : base(options, "NotificationMessage")
        {
        }

        public async Task<IEnumerable<NotificationMessageDto>> GetNotificationMessages(Guid deviceId)
        {
            return await Get<IEnumerable<NotificationMessageDto>>($"notification/{deviceId}");
        }

        public async Task ConfirmNotificationMessages(Guid deviceId)
        {
             await Delete($"notification/{deviceId}");
        }
    }
}