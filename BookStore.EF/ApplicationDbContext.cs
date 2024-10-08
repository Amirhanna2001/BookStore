using BookStore.CORE.Models;
using Microsoft.EntityFrameworkCore;
namespace BookStore.EF;
public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
}