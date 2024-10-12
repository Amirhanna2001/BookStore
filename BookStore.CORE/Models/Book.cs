using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.Models;
public class Book:BaseModel
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [Range(0,100)]
    public byte SalePercentage { get; set; }
    public byte CategoryId { get; set; }
    public Category Category { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }

}