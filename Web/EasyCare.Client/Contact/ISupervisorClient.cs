using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface ISupervisorClient : IBaseCrudClient<SupervisorDto>
    {
        Task<UsersDto> GetSupervisorByEmail(string Email, string DeviceCode = null);
        Task<IEnumerable<SupervisorDto>> GetGroupParticipanById(Guid supervisorId);
    }
}