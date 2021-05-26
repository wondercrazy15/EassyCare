using System;
using EasyCare.Models.Monitoring;

namespace EasyCare.Interface
{
    public interface CameraInterface
    {
        void LaunchCamera(FileFormatEnum imageType, string imageId = null, ISelectedImagepath selectedImagepath = null);
        void LaunchGallery(FileFormatEnum imageType, string imageId = null, ISelectedImagepath selectedImagepath = null);
        void LaunchDocument(ISelectedImagepath selectedImagepath = null);
        void openDocument(string imagePath);
    }

    public interface ISelectedImagepath
    {
        void SelectedImagepath(string imagePath);

    }
}
