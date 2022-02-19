using System.Linq.Expressions;
using LanguageExt;
using static LanguageExt.Prelude;
using LanguageExt.SomeHelp;
using Microsoft.EntityFrameworkCore;
using PuppyPlace.Data;

namespace PuppyPlace.Repository;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetEntities();
    IQueryable<T> GetByWhere(Expression<Func<T, bool>> predicate);
    Task<Unit> AddAsync(T entity);
    Task<Unit> DeleteAsync(T entity);
    Task<Option<T>> FindAsync(int id);
    Task<Unit> UpdateAsync(T entity);
    Task<int> SaveAsync();
}

public abstract class GenericRepository<T> :
    IGenericRepository<T> where T : class
{
    protected PuppyPlaceContext Context;

    public GenericRepository(PuppyPlaceContext context)
    {
        Context = context;
    }
    
    public IQueryable<T> GetEntities()
    {
        return Context.Set<T>().AsNoTracking();
    }

    public IQueryable<T> GetByWhere(Expression<Func<T, bool>> predicate)
    {
        var result = Context.Set<T>().Where(predicate).AsNoTracking();

        return result;
    }

    public virtual async Task<Unit> AddAsync(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        await Context.SaveChangesAsync();
        return Unit.Default;
        
    }

    public virtual async Task<Unit> DeleteAsync(T entity)
    {
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
        return Unit.Default;
    }

    public virtual async Task<Option<T>> FindAsync(int id)
    {
        var result = await Context.Set<T>().FindAsync(id);
        if (result == null)
            return Option<T>.None;

        return result.ToSome();
    }
    
    public virtual async Task<Unit> UpdateAsync(T entity)
    {
        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync();
        return Unit.Default;
    }

    public virtual async Task<int> SaveAsync()
    {
        return await Context.SaveChangesAsync();
    }
}
