using Microsoft.AspNetCore.Http;

namespace BookStore.CORE.DTOs;
public class BookDTO:BaseBookDTO
{
    public IFormFile? Image;
}