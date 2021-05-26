using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace EasyCare.Core.Services.File
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileService(ILogger<FileService> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> SaveFile(string Image, String FileDirectory, Guid Id)
        {
            string webRootPath = "", folderPath = "", fileName = "";

            try
            {
                webRootPath = _hostingEnvironment.WebRootPath;
                byte[] bytes = Convert.FromBase64String(Image);
                if (string.IsNullOrWhiteSpace(webRootPath))
                {
                    webRootPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");
                }
                folderPath = Path.Combine(webRootPath, "EasyCare", FileDirectory);
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                string imageName = Id + ".jpg";
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    FileStream file = new FileStream(Path.Combine(folderPath, imageName), FileMode.Create, FileAccess.Write);
                    ms.WriteTo(file);
                    file.Close();
                    ms.Close();
                }
                fileName = "/EasyCare/" + FileDirectory + "/" + imageName;

                return fileName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error store file");
                return null;
            }
        }

        public async Task<bool> DeleteFile(String FileDirectory, Guid Id)
        {
            string webRootPath = "";

            try
            {
                webRootPath = _hostingEnvironment.WebRootPath;
                string imageName = Id + ".jpg";

                string filePath = Path.Combine(webRootPath, "EasyCare", FileDirectory, imageName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error store file");
                return false;
            }
        }
    }
}
