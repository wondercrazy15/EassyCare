using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class Supervisor : BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EMail { get; set; }
        public string Image { get; set; }
        public bool IsSenior { get; set; }
        public bool IsModerator { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomSupervisor
    {
        public Guid SupervisorId { get; set; }
        public Guid DeviceId { get; set; }
        public Guid Seniors { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EMail { get; set; }
        public Int32 PostCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
    }
}
