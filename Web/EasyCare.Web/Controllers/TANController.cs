using System;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TANController : CrudController<TAN, TANDto>
    {
        private readonly INotificationService _notificationService;
        public TANController(ITANRepository repository, ILogger<TANController> logger, IMapper mapper, INotificationService notificationService) : base(repository, logger, mapper)
        {
            _notificationService = notificationService;
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var entity = await (_repository as TANRepository).GetByCode(code);
                var dto = _mapper.Map<TANDto>(entity);
                return Ok(dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, code);
                return BadRequest();
            }
        }

        [HttpDelete("DeleteTANByCode/{code}")]
        public async Task<IActionResult> DeleteTANByCode(string code)
        {
            try
            {
               
                var entity = await (_repository as TANRepository).GetByNotificationTagCode(code);
                if(entity != null)
                {
                    var success = await _notificationService.DeleteRegistrationDevice(entity.Code,code);
                    if (success)
                    {
                        var result = await _repository.Delete(entity.Id);
                        if (result != null)
                        {
                            var dto = _mapper.Map<TANDto>(entity);
                            return Ok(dto);
                        }
                    }
                    return NotFound();
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, code);
                return BadRequest();
            }
        }
    }
}
