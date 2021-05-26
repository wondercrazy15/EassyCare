using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyCare.Client.Contact.Base
{
    public interface IBaseCrudClient<TDto> where TDto : class
    {
        Task<IEnumerable<TDto>> GetItems();

        Task<TDto> GetItem(Guid id);

        Task<TDto> DeleteItem(Guid id);

        Task<TDto> PostItem(TDto item);

        Task<TDto> PutItem(Guid id, TDto item);
    }
}