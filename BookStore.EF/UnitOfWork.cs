﻿using BookStore.CORE;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using BookStore.EF.Repositories;

namespace BookStore.EF;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    //public IGenericRepository<Category> Categories { get; private set; }
    public ICategoryRepository Categories { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        //Categories =new GenericRepository<Category>(_context);
        Categories = new CategoryRepository(_context);
    }
    public int SaveChanges() => _context.SaveChanges();
    public void Dispose()=>_context.Dispose();

}