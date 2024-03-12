using BlogMVC.Models;

namespace BlogMVC.Repositories
{
    public class PostRepository : IBlogRepository<Post>
    {
        public Task Add(Post entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Post?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Post>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Post?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Post entity)
        {
            throw new NotImplementedException();
        }
    }

}
