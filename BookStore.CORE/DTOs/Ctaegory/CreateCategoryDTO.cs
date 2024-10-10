using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.DTOs;
public class CreateCategoryDTO
{
    [MaxLength(100)]
    public string Name { get;set; }
    public IFormFile? Image { get; set; } 
}
