using System;

namespace EasyCare.Core.Dto
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public Guid SeniorId { get; set; }
        public Guid SupervisorId { get; set; }
        public string Descriptions { get; set; }
        public string NotificationTagCode { get; set; }
    }
}
