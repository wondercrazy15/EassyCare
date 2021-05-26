using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using System;


namespace EasyCare.Client.Clients
{
    public class SupervisorClient : BaseCrudClient<SupervisorDto>, ISupervisorClient
    {
        public SupervisorClient(Options options) : base(options, "Supervisor")
        {
           
        }

        public async Task<UsersDto> GetSupervisorByEmail(string Email, string DeviceCode = null)
        {
            return await Get<UsersDto>($"GetSupervisorByEmail/{Email}/{DeviceCode}");
        }

        public async Task<IEnumerable<SupervisorDto>> GetGroupParticipanById(Guid supervisorId)
        {
            return await Get<IEnumerable<SupervisorDto>>($"GetGroupParticipanById/{supervisorId}");
        }
    }
}