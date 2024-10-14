﻿using BookStore.CORE.Consts;
using System.Linq.Expressions;

namespace BookStore.CORE.Repositories;
public interface IGenericRepository<T> where T : class
{
    T GetById(int id);
    Task<T> GetByIdAsync(int id);
    IEnumerable<T> GetAll(int pageSize,int pageNumber);
    IEnumerable<T> GetAll(int pageSize,int pageNumber, string[] includes = null);
    IEnumerable<T> GetAll(Expression<Func<T, bool>> criteria, int pageSize, int pageNumber, string[] includes = null);
    Task<IEnumerable<T>> GetAllAsync();
    T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
    Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
        Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
    Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
        Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
    T Add(T entity);
    Task<T> AddAsync(T entity);
    IEnumerable<T> AddRange(IEnumerable<T> entities);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
    T Update(T entity);
    void Delete(T entity,string fileFath = null);
    void DeleteRange(IEnumerable<T> entities);
    void Attach(T entity);
    void AttachRange(IEnumerable<T> entities);
    int Count();
    int Count(Expression<Func<T, bool>> criteria);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> criteria);
}