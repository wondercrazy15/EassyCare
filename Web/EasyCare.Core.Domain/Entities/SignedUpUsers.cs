using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class SignedUpUsers : BaseEntity
    {
        public string PwHash { get; set; }
        public Guid SupervisorId { get; set; }
        public string EMail { get; set; }
    }
}
