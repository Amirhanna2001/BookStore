using BookStore.CORE.Models;

namespace BookStore.CORE.Repositories;
public interface ICategoryRepository:IGenericRepository<Category>
{
    Category GetById(byte id);
    Task<Category> GetByIdAsync(byte id);
} 
