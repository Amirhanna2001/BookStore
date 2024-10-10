using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
namespace BookStore.EF.Repositories;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IImageProcesses _imageProcesses;
    public CategoryRepository(ApplicationDbContext context, IImageProcesses imageProcesses) : base(context,imageProcesses)
    {
        _context = context;
        _imageProcesses = imageProcesses;
    }
    public Category GetById(byte id)
    {
        return _context.Categories.Find(id);
    }
    public async Task<Category> GetByIdAsync(byte id) => await _context.Categories.FindAsync(id);
}