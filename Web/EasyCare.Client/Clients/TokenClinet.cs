using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Client.Clients
{
    public class TokenClient : BaseCrudClient<TokenDto>, ITokenClient
    {
        public TokenClient(Options options) : base(options, "Token")
        {
        }

        public async Task<TokenDto> Token(LoginDto item)
        {
            return await Post<LoginDto,TokenDto>($"Token", item);
        }
    }
}
