    using System;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
    using AutoMapper;
    using EasyCare.Core.Domain.Entities;
    using EasyCare.Core.Dto;
    using EasyCare.Core.Infrastructure.Repository;
    using EasyCare.Web.Controllers.Base;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    namespace EasyCare.Web
    {
        [Route("api/[controller]")]
        [ApiController]
        public class NotificationMessageController : CrudController<NotificationMessage, NotificationMessageDto>
        {
            public NotificationMessageController(INotificationMessageRepository repository, ILogger<NotificationMessageController> logger, IMapper mapper) : base(repository, logger, mapper)
            {
            }

            [HttpGet("notification/{deviceId}")]
            public async Task<IActionResult> GetNotifications(Guid deviceId)
            {
                try
                {
                    var entities = await (_repository as NotificationMessageRepository).GetNotificationMessage(deviceId);
                    var dtos = _mapper.Map<IEnumerable<NotificationMessageDto>>(entities);
                    return Ok(dtos);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest();
                }
            }

        [HttpDelete()]
        [Route("notification/{deviceId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
            public async Task<IActionResult> ConfirmNotifications([Required][FromRoute] Guid deviceId)
            {
                try
                {
                    await (_repository as NotificationMessageRepository).ConfirmNotificationMessages(deviceId);
                    return Ok();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return BadRequest();
                }
            }

        }
    }
