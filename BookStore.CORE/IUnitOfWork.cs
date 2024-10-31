using BookStore.CORE.Models;
using BookStore.CORE.Repositories;

namespace BookStore.CORE;
public interface IUnitOfWork:IDisposable
{
    IGenericRepository<Book> Books { get; }
    IGenericRepository<Author> Authors { get; }
    ICategoryRepository Categories { get; }
    //IAuthRepository authRepository { get; }

    int SaveChanges();
}
