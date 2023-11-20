using Wba.WebFoods.Core.Entities;
using Wba.WebFoods.Web.Areas.Admin.ViewModels;
using Wba.WebFoods.Web.Services.Interfaces;

namespace Wba.WebFoods.Web.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileService> _logger;

        public FileService(IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public bool DeleteFile(string filename)
        {
            //rebuild image path
            var imagePath = Path
                .Combine(_webHostEnvironment.WebRootPath, "images", filename);
            //delete the image
            try
            {
                System.IO.File.Delete(imagePath);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                _logger.LogError(fileNotFoundException.Message);
                return false;
            }
            return true;
        }

        public string StoreFile(IFormFile file)
        {
            //create a unique filename
            var filename = $"{Guid.NewGuid()}_{file.FileName}";
            //create the folderpath
            var pathToFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            //test if path exists
            if (!Directory.Exists(pathToFolder))
            {
                Directory.CreateDirectory(pathToFolder);
            }
            //create the full path
            var pathToFile = Path.Combine(pathToFolder, filename);
            //copy the uploaded file to new path
            using (var fileStream = new FileStream(pathToFile, FileMode.Create))
            {
                //copy the file from the formfile to the path
                try
                {
                    file.CopyTo(fileStream);
                }
                catch (FileNotFoundException exception)
                {
                    _logger.LogCritical(exception.Message);
                    return "Error";
                }
            }
            //put the filename in database
            return filename;
        }
    }
}
