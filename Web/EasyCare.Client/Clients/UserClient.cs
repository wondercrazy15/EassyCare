using System;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    //public class UserClient
    //{
    //    public UserClient()
    //    {
    //    }
    //}

    public class UserClient : BaseCrudClient<UserDto>, IUserClient
    {
        public UserClient(Options options) : base(options, "User")
        {
        }

        public async Task<SupervisorDto> SetSenior(SetUserDto item)
        {
            return await Put<SetUserDto, SupervisorDto>($"SetSenior", String.Empty, item);
        }

        public async Task<SupervisorDto> SetModerator(SetUserDto item)
        {
            return await Put<SetUserDto, SupervisorDto>($"SetModerator", String.Empty, item);
        }

        public async Task<UserDto> AddSenior(UserDto item)
        {
            return await Post<UserDto, UserDto>($"AddSenior", item);
        }

        //public async Task<SupervisorDto> UpdateProfile(SupervisorDto item)
        //{
        //    //return await Put<SupervisorDto, SupervisorDto>(item.Id.ToString(), String.Empty, item);
        //    return await Put<SupervisorDto, SupervisorDto>($"UpdateProfile", string.Empty, item);
        //}

    }
}