using System;
namespace EasyCare.Core.Dto
{
    public class NotificationMessageDto
    {

        public Guid DeviceId { get; set; }

        public Guid NotificationId { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}
