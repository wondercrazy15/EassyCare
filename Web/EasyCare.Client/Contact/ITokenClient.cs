using EasyCare.Core.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Client.Contact
{
    public interface ITokenClient
    {
        Task<TokenDto> Token(LoginDto item);
    }
}
