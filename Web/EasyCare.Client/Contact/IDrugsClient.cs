using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Client.Contact
{
    public interface IDrugsClient : IBaseCrudClient<DrugsDto>
    {
        Task<IEnumerable<DrugDto>> GetDrugsByDate(string Date = null, string SeniorId = null);
    }
}
