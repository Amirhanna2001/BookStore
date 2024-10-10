using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.Models;
public class BaseModel
{
    [MaxLength(100)]
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}
