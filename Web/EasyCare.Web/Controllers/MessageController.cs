using System;
using System.Collections.Generic;
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
    public class MessageController : CrudController<Message, MessageDto>
    {
        public MessageController(IMessageRepository repository, ILogger<MessageController> logger, IMapper mapper) : base(repository, logger, mapper)
        {
        }

        [HttpGet("chat/{supervisorId}/{receiverId}")]
        public async Task<IActionResult> GetChat(Guid supervisorId, Guid receiverId)
        {
            try
            {
                var entities = await (_repository as IMessageRepository).GetChatMessages(supervisorId, receiverId);
                var dtos = _mapper.Map<IEnumerable<MessageDto>>(entities);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
    }
}