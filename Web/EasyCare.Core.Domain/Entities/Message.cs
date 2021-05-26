using System;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    public class Message : BaseEntity
    {
        public string Content { get; set; }

        public Guid AuthorId { get; set; }

        public Guid ReceiverId { get; set; }

        public DateTime SendDate { get; set; }
    }
}