using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.CORE.Models;
public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public byte Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}
