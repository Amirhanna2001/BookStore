using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Http;

namespace BookStore.EF.Repositories;

public class ImagesProcesses : IImageProcesses
{
    public bool IsAvailableExtension(IFormFile image)
    {
        if(image != null && image.Length > 0)
        {
            string extension = Path.GetExtension(image.FileName).ToUpper();
            if(extension == ".PNG" ||  extension == ".JPG")
                return true;
        }

        return false;
    }

    public async Task<string> StoreImage(IFormFile image, string path)
    {
        string imagePath = "";
        if(image != null && image.Length > 0)
        {
            string uniqueName = Guid.NewGuid().ToString() + "_" + image.FileName; 
            string filePath = Path.Combine(path, uniqueName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                await image.CopyToAsync(fileStream);

            imagePath = uniqueName;
        }
        return imagePath;
    }
    public bool DeleteImage(string path)
    {
        if(!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        {
            File.Delete(path);
            return true;
        }
        return false;
    }
}