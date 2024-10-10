using BookStore.CORE.Models;
using BookStore.CORE.Repositories;

namespace BookStore.CORE;
public interface IUnitOfWork:IDisposable
{
    IGenericRepository<Author> Authors { get; }
    ICategoryRepository Categories { get; }

    int SaveChanges();
}
