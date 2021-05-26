using EasyCare.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Domain.Entities
{
    public class Participant : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Guid ParticipationId { get; set; }
        public bool IsModerator { get; set; }
    }
}
