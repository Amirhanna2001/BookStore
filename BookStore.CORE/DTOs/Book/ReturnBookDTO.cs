namespace BookStore.CORE.DTOs;
public class ReturnBookDTO:BaseBookDTO
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryName { get; set; }
    public string AuthorName { get; set; }
}