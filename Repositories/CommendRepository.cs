using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Repositories
{
    public class CommendRepository : IBlogRepository<Comment>
    {
        private readonly ApplicationDbContext _context;

        public CommendRepository(ApplicationDbContext context)
        {
            _context = context;
        }






        public async Task Add(Comment entity)
        {
             _context.Comments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var blog = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            
            _context.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public Task<Comment?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Comment?> GetById(int id)
        {
            return await _context.Comments.Include(c => c.Author).Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Comment entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

}
