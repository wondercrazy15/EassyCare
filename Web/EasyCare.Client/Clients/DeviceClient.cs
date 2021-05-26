using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class DeviceClient : BaseCrudClient<DeviceDto>, IDeviceClient
    {
        public DeviceClient(Options options) : base(options, "Device")
        {
        }

        public async Task<IEnumerable<DeviceDto>> GetBySupervisorId(Guid supervisorId)
        {
            return await Get<IEnumerable<DeviceDto>>($"supervisor/{supervisorId}");
        }
    }
}