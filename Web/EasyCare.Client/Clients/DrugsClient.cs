using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Client.Clients
{
    public class DrugsClient : BaseCrudClient<DrugsDto>, IDrugsClient
    {
        public DrugsClient(Options options) : base(options, "Drugs")
        {
        }

        public async Task<IEnumerable<DrugDto>> GetDrugsByDate(string Date = null, string SeniorId = null)
        {
            return await Get<IEnumerable<DrugDto>>($"GetDrugsByDate/{Date}/{SeniorId}");
        }
    }
}
