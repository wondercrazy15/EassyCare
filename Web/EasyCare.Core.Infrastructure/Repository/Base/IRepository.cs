using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Infrastructure.Repository.Base
{
    public interface IRepository<TEntity> where  TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> Get();

        Task<TEntity> Get(Guid id);

        Task<bool> Update(Guid id, TEntity entity);

        Task<TEntity> Add(TEntity entity);

        Task<TEntity> Delete(Guid id);
    }
}