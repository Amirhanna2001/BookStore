using BookStore.CORE;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using BookStore.EF.Repositories;

namespace BookStore.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IImageProcesses _imageProcesses;
    public ICategoryRepository Categories { get; private set; }
    public IGenericRepository<Book> Books { get; private set; }
    public IGenericRepository<Author> Authors { get; private set; }
    public IAuthRepository AuthRepository { get; private set; }
    public UnitOfWork(ApplicationDbContext context, IImageProcesses imageProcesses)
    {
        _context = context;
        Books = new GenericRepository<Book>(_context,imageProcesses);
        Authors = new GenericRepository<Author>(_context,imageProcesses);
        Categories = new CategoryRepository(_context, imageProcesses);
        _imageProcesses = imageProcesses;
    }
    public int SaveChanges() => _context.SaveChanges();
    public void Dispose()=>_context.Dispose();

}
