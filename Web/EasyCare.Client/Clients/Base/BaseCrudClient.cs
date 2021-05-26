using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;

namespace EasyCare.Client.Clients.Base
{
    public abstract class BaseCrudClient<TDto> : BaseClient, IBaseCrudClient<TDto> where TDto : class
    {
        protected BaseCrudClient(Options options, string service) : base(options, service)
        {
        }
        
        public async Task<IEnumerable<TDto>> GetItems()
        {
            return await Get<IEnumerable<TDto>>();
        }

        public async Task<TDto> GetItem(Guid id)
        {
            return await Get<TDto>(id.ToString());
        }

        public async Task<TDto> DeleteItem(Guid id)
        {
            return await Delete<TDto>(id.ToString());
        }

        public async Task<TDto> PostItem(TDto item)
        {
            return await Post<TDto, TDto>(String.Empty, item);
        }

        public async Task<TDto> PutItem(Guid id, TDto item)
        {
            return await Put<TDto, TDto>(id.ToString(), String.Empty, item);
        }
    }
}