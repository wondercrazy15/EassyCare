using System.Threading.Tasks;
using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using Microsoft.AspNetCore.Http;

namespace EasyCare.Client.Clients
{
    public class TanClient : BaseCrudClient<TANDto>, ITanClient
    {
        public TanClient(Options options) : base(options, "TAN")
        {
        }

        public async Task<TANDto> GetByCode(string code)
        {
            return await Get<TANDto>($"code/{code}");
        }

        public async Task<TANDto> DeleteTANByCode(string code)
        {
            return await Delete<TANDto>($"DeleteTANByCode/{code}");
        }
    }
}