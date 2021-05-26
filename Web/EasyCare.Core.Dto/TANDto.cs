using System;

namespace EasyCare.Core.Dto
{
    public class TANDto
    {
        public Guid TanId { get; set; }
        public string Code { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime Expiration { get; set; }
    }
}


