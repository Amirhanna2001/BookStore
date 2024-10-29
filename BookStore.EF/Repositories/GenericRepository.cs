using BookStore.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.CORE.Repositories;
public class GenericRepository<T>:IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly IImageProcesses _imageProcesses;

    public GenericRepository(ApplicationDbContext context, IImageProcesses imageProcesses )
    {
        _context = context;
        _imageProcesses = imageProcesses;
    }

    public IEnumerable<T> GetAll(int pageSize, int pageNumber)
        => _context.Set<T>().Skip(pageNumber*pageSize).Take(pageSize).ToList();

    public async Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageNumber)
        =>await _context.Set<T>().Skip(pageNumber*pageSize).Take(pageSize).ToListAsync();

    public  IEnumerable<T> GetAll(int pageSize, int pageNumber, string[] includes = null)
    {
        IQueryable<T> query =  _context.Set<T>().Skip(pageNumber * pageSize).Take(pageSize);
        if(includes != null )
            foreach(var include in includes)
                query = query.Include(include);

        return  query.ToList();
    }
    public async Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageNumber, string[] includes = null)
    {
        IQueryable<T> query =  _context.Set<T>().Skip(pageNumber * pageSize).Take(pageSize);
        if(includes != null )
            foreach(var include in includes)
                query = query.Include(include);

        return await query.ToListAsync();
    }
    public  IEnumerable<T> FindAll(Expression<Func<T,bool>> criteria, int pageSize, int pageNumber, string[] includes = null)
    {
        IQueryable<T> query =  _context.Set<T>().Where(criteria).Skip(pageNumber * pageSize).Take(pageSize);
        if(includes != null )
            foreach(var include in includes)
                query = query.Include(include);

        return  query.ToList();
    }
    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T,bool>> criteria, int pageSize, int pageNumber, string[] includes = null)
    {
        IQueryable<T> query =  _context.Set<T>().Where(criteria).Skip(pageNumber * pageSize).Take(pageSize);
        if(includes != null )
            foreach(var include in includes)
                query = query.Include(include);

        return await query.ToListAsync();
    }

    public T GetById(int id) => _context.Set<T>().Find(id);

    public async Task<T> GetByIdAsync(int id)=>await _context.Set<T>().FindAsync(id);

    public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.SingleOrDefault(criteria);
    }

    public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.SingleOrDefaultAsync(criteria);
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        return entities;
    }

    public T Update(T entity)
    {
        _context.Update(entity);
        return entity;
    }

    public void Delete(T entity, string filePath = null)
    {
        if(!string.IsNullOrEmpty(filePath))
            _imageProcesses.DeleteImage(filePath);

        _context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
        =>_context.Set<T>().RemoveRange(entities);

    public void Attach(T entity) => _context.Set<T>().Attach(entity);

    public void AttachRange(IEnumerable<T> entities) =>_context.Set<T>().AttachRange(entities);
    public int Count() =>_context.Set<T>().Count();
    public int Count(Expression<Func<T, bool>> criteria) => _context.Set<T>().Count(criteria);

    public async Task<int> CountAsync()=>await _context.Set<T>().CountAsync();

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        =>await _context.Set<T>().CountAsync(criteria);
}