namespace BookStore.CORE.Models;
public class JWT
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public byte DurationInDays { get; set; }
}