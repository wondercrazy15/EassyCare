using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EasyCare.Client.Contact
{
    public interface IDeviceClient : IBaseCrudClient<DeviceDto>
    {
        Task<IEnumerable<DeviceDto>> GetBySupervisorId(Guid supervisorId);
    }
}