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
using EasyCare.Web.Controllers.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace EasyCare.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : CrudController<Group, GroupDto>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IFileService _fileService;
        public GroupController(IGroupRepository repository, ILogger<GroupController> logger, IMapper mapper, IParticipantRepository participantRepository, ISupervisorRepository supervisorRepository, IFileService fileService)
            : base(repository, logger, mapper)
        {
            _participantRepository = participantRepository;
            _supervisorRepository = supervisorRepository;
            _fileService = fileService;
        }
        [HttpPost]
        public override async Task<ActionResult<GroupDto>> PostItem(GroupDto item)
        {
            try
            {
                Group groupExist = null;
                item.Id = Guid.NewGuid();
                string fileDirectory = "Groups";

                var supervisorExist = await _participantRepository.GetBySupervisorId(Guid.Parse(item.SupervisorId));
                if (supervisorExist != null)
                {
                    groupExist = await _repository.Get(supervisorExist.GroupId);
                }
                if (groupExist == null)
                {
                    item.Image = await _fileService.SaveFile(item.Image, fileDirectory, item.Id);
                    item.CreatedDate = DateTime.Now;
                    item.Isactive = true;

                    var entity = _mapper.Map<Group>(item);
                    var objGroup = await _repository.Add(entity);
                    if (objGroup != null)
                    {
                        Guid SupervisorID = Guid.Parse(item.SupervisorId);
                        Core.Domain.Entities.Participant objParticipant = new Core.Domain.Entities.Participant();
                        objParticipant.ParticipationId = SupervisorID;
                        objParticipant.GroupId = objGroup.Id;
                        objParticipant.IsModerator = true;
                        var result = await _participantRepository.Add(objParticipant);

                        var objSupervisor = _supervisorRepository.Get(SupervisorID);
                        if (objSupervisor != null)
                        {
                            objSupervisor.Result.IsModerator = true;
                            await _supervisorRepository.Update(objSupervisor.Result.Id, objSupervisor.Result);
                        }

                    }
                    return Ok(entity);
                }
                else
                {
                    return Conflict();
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }
        [HttpGet()]
        [Route("GetGroupsBySupervisorId/{SupervisorId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetGroupsBySupervisorId(Guid SupervisorId)
        {
            try
            {
                var objSupervisor = await _participantRepository.GetBySupervisorId(SupervisorId);
                if (objSupervisor != null)
                {
                    var entities = await _repository.Get(objSupervisor.GroupId);
                    if (entities != null)
                    {
                        var result = _mapper.Map<GroupDto>(entities);
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
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult<GroupDto>> PutItem(Guid id, GroupDto item)
        {
            try
            {
                string fileDirectory = "Groups";
                var groupExist = await _repository.Get(item.Id);
                if (groupExist != null)
                {
                    var deleteFile =await _fileService.DeleteFile(fileDirectory,id);
                    if (deleteFile)
                    {
                        item.Image = await _fileService.SaveFile(item.Image, fileDirectory, id);
                        item.ModifiedDate = DateTime.Now;

                        var entity = _mapper.Map<Group>(item);
                        var result = await _repository.Update(entity.Id, entity);
                        if (result)
                        {
                           var objGroup= await _repository.Get(item.Id);
                            return Ok(objGroup);
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
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }
    }
}