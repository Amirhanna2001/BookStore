using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.DTOs;
public class BaseBookDTO
{
    [MaxLength(100)]
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [Range(0, 100)]
    public byte SalePercentage { get; set; }
    public byte CategoryId { get; set; }
    public int AuthorId { get; set; }
}