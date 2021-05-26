using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class SeniorClient : BaseCrudClient<SeniorDto>, ISeniorClient
    {
        public SeniorClient(Options options) : base(options, "Senior")
        {
        }
    }
}