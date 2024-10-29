using System.ComponentModel.DataAnnotations;

namespace BookStore.CORE.DTOs;
public class RegisterDTO
{
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
    public string UserName { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
    [MaxLength(100)]
    public string Password { get; set; }

}
