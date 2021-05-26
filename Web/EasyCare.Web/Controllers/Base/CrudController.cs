using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EasyCare.Core.Domain.Entities.Base;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EasyCare.Web.Controllers.Base
{
    public abstract class CrudController<TEntity, TDto> : ControllerBase where TEntity : BaseEntity
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        
        public CrudController(IRepository<TEntity> repository, ILogger logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetItems()
        {
            try
            {
                var items = await _repository.Get();
                var dtos = _mapper.Map<IEnumerable<TDto>>(items);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> GetItem(Guid id)
        {
            try
            {
                var item = await _repository.Get(id);
                if (item is null)
                {
                    return NotFound();
                }

                var dto = _mapper.Map<TDto>(item);
                return Ok(dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, id);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TDto>> DeleteItem(Guid id)
        {
            try
            {
                var entity = await _repository.Delete(id);
                var dto = _mapper.Map<TDto>(entity);
                return Ok(dto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, id);
                return BadRequest();
            }
        }
        
        [HttpPost]
        public virtual async Task<ActionResult<TDto>> PostItem(TDto item)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(item);
                await _repository.Add(entity);
                return CreatedAtAction("GetItem", new { id = entity.Id }, entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, item);
                return BadRequest();
            }
        }

        

       

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TDto>> PutItem(Guid id, TDto item)
        {
            try
            {
                var itemFromDB = await _repository.Get(id);
                if (itemFromDB is null)
                {
                    return NotFound();
                }

                var entity = _mapper.Map<TEntity>(item);
                var result = await _repository.Update(id, entity);
                if (result)
                {
                    return CreatedAtAction("GetItem", new { id = entity.Id }, entity);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message, id, item);
                return BadRequest();
            }
        }
    }
}