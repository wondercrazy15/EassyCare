using System;

namespace EasyCare.Core.Dto
{
    public class SignedUpUsersDto
    {
        public Guid Id { get; set; }
        public string PwHash { get; set; }
        public Guid SupervisorId { get; set; }
        public string EMail { get; set; }
    }
}
