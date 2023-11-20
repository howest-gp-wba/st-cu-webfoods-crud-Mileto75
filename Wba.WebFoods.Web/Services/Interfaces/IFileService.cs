namespace Wba.WebFoods.Web.Services.Interfaces
{
    public interface IFileService
    {
        string StoreFile(IFormFile file);
        bool DeleteFile(string filename);
    }
}
