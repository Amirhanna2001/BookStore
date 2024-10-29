using Microsoft.AspNetCore.Http;

namespace BookStore.CORE.Repositories;

public interface IImageProcesses
{
    Task<string> StoreImage(IFormFile image, string path);
    bool IsAvailableExtension(IFormFile image);
    bool DeleteImage(string path);

}