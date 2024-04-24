using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Repositories
{
    public class PostRepository : IBlogRepository<Post>
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Post entity)
        {
            _context.Posts.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = await GetById(id);

            if (post is not null)
            {
                _context.Remove(post);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Post?> FindById(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Post>?> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post?> GetById(int id)
        {
            return await _context.Posts.Include(p => p.Author)
                                       .Include(p => p.Blog)
                                       .Include(p => p.Tags)
                                       .Include(p => p.Comments.OrderByDescending(c => c.Created))
                                       .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Update(Post entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

}
