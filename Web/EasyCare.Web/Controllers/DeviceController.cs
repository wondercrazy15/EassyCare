using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : CrudController<Device, DeviceDto>
    {
        public DeviceController(IDeviceRepository repository, ILogger<DeviceController> logger, IMapper mapper)
            : base(repository, logger, mapper)
        {
        }

        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetBySupervisorId(Guid supervisorId)
        {
            try
            {
                var entities = await (_repository as IDeviceRepository).GetBySupervisorId(supervisorId);
                var dtos = _mapper.Map<IEnumerable<DeviceDto>>(entities);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
