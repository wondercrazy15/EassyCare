using System;
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
    public class SignedUpUsersController : CrudController<SignedUpUsers, SignedUpUsersDto>
    {
        public SignedUpUsersController(ISingedUpUserRepository repository, ILogger<SignedUpUsersController> logger, IMapper mapper) 
            : base(repository, logger, mapper)
        {
        }

        [HttpPost]
        public override async Task<ActionResult<SignedUpUsersDto>> PostItem(SignedUpUsersDto item)
        {
            try
            {
                var user = await (_repository as ISingedUpUserRepository).GetByEmail(item.EMail);
                if (user is null)
                {
                    var entity = _mapper.Map<SignedUpUsers>(item);
                    await _repository.Add(entity);
                    return CreatedAtAction("GetItem", new { id = entity.Id }, entity);
                }
                
                // TODO Thats must be changes, becasue it is confused :(
                if (user.PwHash == item.PwHash)
                {
                    var resultDto = _mapper.Map<SignedUpUsersDto>(user);
                    return Ok(resultDto);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }
    }
}
