using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ISeniorRepository : IRepository<Senior>
    {
        
    }
    
    public class SeniorRepository : BaseRepository<Senior>, ISeniorRepository
    {
        public SeniorRepository(DatabaseContext context) : base(context)
        {
        }
    }
}