using System;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class NotificationMessage : BaseEntity
    {
        public Guid DeviceId { get; set; }
        public Guid NotificationId { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
