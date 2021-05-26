using System;
using System.IO;
using System.Threading.Tasks;

namespace EasyCare.DataService
{
    public interface IImageChooser
    {
        Task<bool> StartImageChooser();
        Stream GetImage();
    }
}
