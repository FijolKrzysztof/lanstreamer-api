namespace lanstreamer_api.Data.Utils;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T?> Reload(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task Delete(int id);
    Task DeleteMany(IEnumerable<int> ids);
}
