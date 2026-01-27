using System.Linq.Expressions;

namespace Domain.Base.Interface;

public interface IRepositoryGeneric<T> where T : EntityBase
{
    T Find(object id);
    Task<T> FindAsync(object id);
    void Add(T entity);
    void Delete(T entity);
    void AddRange(List<T> entities);
    void DeleteRange(List<T> entities);
    IEnumerable<T> GetAll();
    Task<IEnumerable<T>> GetAllAsync();
    T FindFirstOrDefault(Expression<Func<T, bool>> predicate, string includeProperties = "");
    IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
    IQueryable<T> FindBy(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
}