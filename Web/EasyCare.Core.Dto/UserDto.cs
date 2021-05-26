using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EMail { get; set; }
        public string Code { get; set; }
        public string PwHash { get; set; }
        public string DeviceCode { get; set; }
    }

    public class UsersDto
    {
        public SupervisorDto supervisor { get; set; }
        public DeviceDto device { get; set; }
        public SeniorDto senior { get; set; }
    }

    public class SetUserDto
    {
        public Guid GroupId { get; set; }
        public Guid ParticipantId { get; set; }
    }

}
