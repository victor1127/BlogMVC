using BlogMVC.Models;

namespace BlogMVC.Repositories
{
    public class CommendRepository : IBlogRepository<Comment>
    {
        public Task Add(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Comment entity)
        {
            throw new NotImplementedException();
        }
    }

}
