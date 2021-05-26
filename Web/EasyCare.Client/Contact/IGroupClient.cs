using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Client.Contact
{
    public interface IGroupClient : IBaseCrudClient<GroupDto>
    {
        Task<GroupDto> GetGroupsBySupervisorId(Guid SupervisorId);
    }
}
