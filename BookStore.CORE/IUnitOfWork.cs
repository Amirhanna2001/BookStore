using BookStore.CORE.Models;
using BookStore.CORE.Repositories;

namespace BookStore.CORE;
public interface IUnitOfWork:IDisposable
{
    //IGenericRepository<Category> Categories { get; }
    ICategoryRepository Categories { get; }
    int SaveChanges();
}
