namespace DaccApi.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> SaveImageFileAsync(IFormFile file);
    }
}