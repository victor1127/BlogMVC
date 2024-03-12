namespace BlogMVC.Repositories
{
    public interface IBlogRepository<T>
    {
        Task<T?> GetById(int id);
        Task<T?> FindById(int id);  
        Task<IEnumerable<T>?> GetAll();
        Task Add(T entity); 
        Task Update(T entity);
        Task Delete(int id);

    }
}
