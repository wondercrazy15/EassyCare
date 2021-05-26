using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Dto;
using EasyCare.Core.Infrastructure.Repository;
using EasyCare.Core.Services.File;
using EasyCare.Core.Services.Notification;
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : CrudController<Supervisor, SupervisorDto>
    {
        private IDeviceRepository _deviceRepository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly ITANRepository _TANRepository;
        private readonly INotificationService _notificationService;
        private readonly IFileService _fileService;
        public SupervisorController(ISupervisorRepository repository, ILogger<SupervisorController> logger, 
            IMapper mapper, IDeviceRepository deviceRepository, 
            IHostingEnvironment hostingEnvironment, ITANRepository TANRepository, 
            INotificationService notificationService, IFileService fileService)
            : base(repository, logger, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
            _TANRepository = TANRepository;
            _notificationService = notificationService;
            _deviceRepository = deviceRepository;
            _fileService = fileService;
        }

        [Authorize]
        [HttpGet("GetSupervisorByEmail/{Email}/{DeviceCode?}")]
        public async Task<IActionResult> GetSupervisorByEmail(string Email, string DeviceCode = null)
        {
            try
            {
                UsersDto objUsers = new UsersDto();
                var entities = await (_repository as ISupervisorRepository).GetSupervisorByEmail(Email);

                if (entities != null)
                {

                    objUsers.supervisor = new SupervisorDto();
                    objUsers.supervisor = _mapper.Map<SupervisorDto>(entities);

                    if (string.IsNullOrEmpty(objUsers.supervisor.Image))
                    {
                        objUsers.supervisor.Image = "/EasyCare/User/" + objUsers.supervisor.Id + ".jpg";
                    }

                    var deviceResult = await _deviceRepository.GetBySupervisorId(entities.Id);
                    if (deviceResult.Count() > 0)
                    {
                        objUsers.device = new DeviceDto();
                        var data = deviceResult.FirstOrDefault();
                        objUsers.device = _mapper.Map<DeviceDto>(data);

                        if (!string.IsNullOrEmpty(DeviceCode))
                        {
                            objUsers.device.NotificationTagCode = await _notificationService.CreateOrUpdateRegistrationDevice(DeviceCode, objUsers.supervisor.Id.ToString());
                            if (!string.IsNullOrEmpty(objUsers.device.NotificationTagCode))
                            {
                                await _TANRepository.AddTAN(objUsers.device.Id, DeviceCode, objUsers.device.NotificationTagCode);
                            }
                        }

                        objUsers.senior = new SeniorDto();
                        var seniorResult = await (_repository as ISupervisorRepository).GetSeniorBySupervisorId(entities.Id);
                        objUsers.senior = _mapper.Map<SeniorDto>(seniorResult);
                    }
                    return Ok(objUsers);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet()]
        [Route("GetGroupParticipanById/{supervisorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetGroupParticipanById(Guid supervisorId)
        {
            try
            {
                var entities = await (_repository as ISupervisorRepository).GetGroupParticipanById(supervisorId);

                if (entities != null)
                {
                    var result = _mapper.Map<IEnumerable<SupervisorDto>>(entities).ToList();
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult<SupervisorDto>> PutItem(Guid id, SupervisorDto item)
        {
            try
            {
                string fileDirectory = "User";
                var supervisorExist = _repository.Get(item.Id);
                if (supervisorExist.Result != null)
                {
                    string imagePath = item.Image;
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        supervisorExist.Result.Image = await _fileService.SaveFile(item.Image, fileDirectory, item.Id);

                        var updateValue = await _repository.Update(item.Id, supervisorExist.Result);
                        if (updateValue)
                        {
                            var entities = _repository.Get(item.Id);
                            var result = _mapper.Map<SupervisorDto>(entities.Result);

                            return Ok(result);
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }
    }
}
