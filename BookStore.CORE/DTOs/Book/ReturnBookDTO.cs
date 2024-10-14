using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.DTOs;
public class ReturnBookDTO
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    [Range(0, 100)]
    public byte SalePercentage { get; set; }
    public byte CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }
}