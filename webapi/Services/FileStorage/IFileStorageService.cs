namespace DaccApi.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> SaveImageFileAsync(IFormFile file);
        Task<string> SaveBase64ImageAsync(string base64Data);
    }
}