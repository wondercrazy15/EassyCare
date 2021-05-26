using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class TAN : BaseEntity
    {
        public string Code { get; set; }
        public Guid DeviceId { get; set; }
        public string NotificationTagCode { get; set; }
        public DateTime Expiration { get; set; }
    }
        
    public class CustomTAN
    {
        public string Tags { get; set; }
    }
}


