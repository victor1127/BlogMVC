using BlogMVC.Models;

namespace BlogMVC.Repositories
{
    public class TagRepository : IBlogRepository<Tag>
    {
        public Task Add(Tag entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tag>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Tag entity)
        {
            throw new NotImplementedException();
        }
    }

}
