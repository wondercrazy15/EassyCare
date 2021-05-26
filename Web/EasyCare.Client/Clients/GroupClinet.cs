using System;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class GroupClinet : BaseCrudClient<GroupDto>, IGroupClient
    {
        public GroupClinet(Options options) : base(options, "Group")
        {
        }

        public async Task<GroupDto> GetGroupsBySupervisorId(Guid SupervisorId)
        {
            return await Get<GroupDto>($"GetGroupsBySupervisorId/{SupervisorId}");
        }
    }
}
