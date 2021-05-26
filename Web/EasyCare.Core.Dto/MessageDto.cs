using System;

namespace EasyCare.Core.Dto
{
    public class MessageDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public Guid AuthorId { get; set; }

        public Guid ReceiverId { get; set; }

        public DateTime SendDate { get; set; }
    }
}