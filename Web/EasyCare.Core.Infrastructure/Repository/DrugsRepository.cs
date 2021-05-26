using EasyCare.Core.Domain.Entities;
using EasyCare.Core.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Core.Infrastructure.Repository
{
    public interface IDrugsRepository : IRepository<Drugs>
    {
        //Can extends for custom method
        Task<IEnumerable<CustomDrugs>> GetDrugsByDate(string Date = null, string SeniorId = null);
    }
    public class DrugsRepository : BaseRepository<Drugs>, IDrugsRepository
    {
        public DrugsRepository(DatabaseContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CustomDrugs>> GetDrugsByDate(string Date = null, string SeniorId = null)
        {
            try
            {
                Guid? SenId = !string.IsNullOrEmpty(SeniorId)? Guid.Parse(SeniorId):Guid.Empty;
                var Drugs = await _context.Set<CustomDrugs>().FromSqlRaw("EXECUTE dbo.GetDrugsByDate @p0,@p1,@p2", Date, SenId, null).ToListAsync();
                return Drugs;
            }
            catch (Exception ex)
            {
                 return null;
            }
        }
    }
}
