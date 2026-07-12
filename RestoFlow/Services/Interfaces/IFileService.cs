using Microsoft.AspNetCore.Http;

namespace RestoFlow.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string destFolder, string[] allowedExtensions, long maxBytes);
        Task<bool> RemoveFileAsync(string path);
    }
}
