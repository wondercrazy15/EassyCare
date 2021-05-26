using System;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Infrastructure.Repository.Base;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorMessageController : CrudController<SensorMessage, SensorMessageDto>
    {
        public SensorMessageController(ISensorMessageRepository repository, ILogger<SensorMessageRepository> logger, IMapper mapper)
            : base(repository, logger, mapper)
        {
        }

        // [HttpGet]
        public override async Task<ActionResult<SensorMessageDto>> GetItem(Guid id)
        {
            try
            {
                var lastMessage = await (_repository as SensorMessageRepository).GetLastByDeviceId(id);
                if (lastMessage is null)
                {
                    return NotFound();
                }

                var resultDto = _mapper.Map<SensorMessageDto>(lastMessage);
                return Ok(resultDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, id);
                return BadRequest();
            }
        }
    }
}
