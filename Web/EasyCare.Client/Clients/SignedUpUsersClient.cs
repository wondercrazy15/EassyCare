using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class SignedUpUsersClient : BaseCrudClient<SignedUpUsersDto>, ISignedUpUsersClient
    {
        public SignedUpUsersClient(Options options) : base(options, "SignedUpUsers")
        {
        }
    }
}