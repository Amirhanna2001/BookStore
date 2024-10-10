using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.CORE.Models;
public class Category:BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public byte Id { get; set; }
    
}
