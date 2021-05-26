using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface INotificationMessageClient : IBaseCrudClient<NotificationMessageDto>
    {
        Task<IEnumerable<NotificationMessageDto>> GetNotificationMessages(Guid deviceId);
        Task ConfirmNotificationMessages(Guid deviceId);
    }
}