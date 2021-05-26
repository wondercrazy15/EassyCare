using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCare.Core.Services.File
{
    public interface IFileService
    {
        Task<string> SaveFile(string Image, String FileDirectory, Guid Id);
        Task<bool> DeleteFile(String FileDirectory, Guid Id);
    }
}
