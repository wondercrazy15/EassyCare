using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface IUserClient : IBaseCrudClient<UserDto>
    {
        //Task<SupervisorDto> UpdateProfile(SupervisorDto item);
        Task<SupervisorDto> SetSenior(SetUserDto item);
        Task<SupervisorDto> SetModerator(SetUserDto item);
        Task<UserDto> AddSenior(UserDto item);
    }
}