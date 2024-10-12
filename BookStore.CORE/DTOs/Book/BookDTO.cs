using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.DTOs;
public class BookDTO:BaseDTO
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [Range(0, 100)]
    public byte SalePercentage { get; set; }
    public byte CategoryId { get; set; }
    public int AuthorId { get; set; }
}