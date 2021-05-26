using System.Threading.Tasks;
using EasyCare.Client.Contact.Base;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Contact
{
    public interface ITanClient : IBaseCrudClient<TANDto>
    {
        Task<TANDto> GetByCode(string code);

        Task<TANDto> DeleteTANByCode(string code);
    }
}