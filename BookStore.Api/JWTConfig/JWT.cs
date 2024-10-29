namespace BookStore.Api.JWTConfig;
public class JWT
{
    public string Key { get; set; }
    public string Issure { get; set; }
    public string Audience { get; set; }
    public byte DurationInDays { get; set; }
}