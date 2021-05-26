using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository.Base
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<TEntity> _set;
        
        public BaseRepository(DatabaseContext context)
        {
            _context = context;
            _set = _context.GetDbSet<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _set.ToListAsync();
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await _set.FindAsync(id);
        }

        public async  Task<TEntity> Delete(Guid id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }

        public async Task<bool> Update(Guid id, TEntity entity)
        {
            if (id == entity.Id)
            {
                var entityFromDB = await _set.FindAsync(id);
                if (entityFromDB != null)
                {
                    _context.Entry(entityFromDB).State = EntityState.Detached;
                    _context.Entry(entity).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}