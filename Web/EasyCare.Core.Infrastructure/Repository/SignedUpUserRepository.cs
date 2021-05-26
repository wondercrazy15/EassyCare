using System;
using System.Threading.Tasks;
using System.Linq;
using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface ISingedUpUserRepository : IRepository<SignedUpUsers>
    {
        Task<SignedUpUsers> GetByEmail(string email);
    }
    
    public class SignedUpUserRepository : BaseRepository<SignedUpUsers>, ISingedUpUserRepository
    {
        public SignedUpUserRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<SignedUpUsers> GetByEmail(string email)
        {
            return await _context.SignedUpUsers.SingleOrDefaultAsync(x => x.EMail == email);
        }
    }
}