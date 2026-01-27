using Domain.Base;
using Domain.Base.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Base;

public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : EntityBase
{
    protected readonly DbContext _db;
    protected readonly DbSet<T> _dbset;

    public RepositoryGeneric(DbContext context)
    {
        _db = context ?? throw new ArgumentNullException(nameof(context));
        _dbset = _db.Set<T>();
    }

    public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
    {
        return _dbset.Where(predicate).AsEnumerable();
    }

    public IQueryable<T> FindBy(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
    {
        IQueryable<T> query = _dbset;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query);
        }
        else
        {
            return query;
        }
    }

    public T? Find(object id)
    {
        return _dbset.Find(id);
    }

    public virtual async Task<T?> FindAsync(object id)
    {
        return await _dbset.FindAsync(id);
    }

    public T? FindFirstOrDefault(Expression<Func<T, bool>> predicate, string includeProperties = "")
    {
        return _dbset.FirstOrDefault(predicate);
    }

    public void Add(T entity)
    {
        _dbset.Add(entity);
        Commit();
    }

    public void Edit(T entity)
    {
        _db.Entry(entity).State = EntityState.Modified;
        Commit();
    }

    public void Delete(T entity)
    {
        _dbset.Remove(entity);
        Commit();
    }

    public void AddRange(List<T> entities)
    {
        _dbset.AddRange(entities);
        Commit();
    }

    public void DeleteRange(List<T> entities)
    {
        _dbset.RemoveRange(entities);
        Commit();
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _dbset.AsEnumerable<T>();
    }
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbset.ToListAsync();
    }

    protected IQueryable<T> AsQueryable()
    {
        return _dbset.AsQueryable();
    }

    public int Commit()
    {
        return _db.SaveChanges();
    }
}