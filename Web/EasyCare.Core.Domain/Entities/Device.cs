using System;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class Device : BaseEntity
    {
        //public Guid SeniorId { get; set; }
        public Guid SupervisorId { get; set; }
        public string Descriptions { get; set; }
    }
}
