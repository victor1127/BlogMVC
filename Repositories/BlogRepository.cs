using BlogMVC.Data;
using BlogMVC.Models;
using BlogMVC.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogMVC.Repositories
{
    public class BlogRepository : IBlogRepository<Blog>
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task Add(Blog entity)
        {
            _context.Blogs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Blog>?> GetAll()
        {
            return await _context.Blogs.Include(b => b.Author).ToListAsync();
        }

        public async Task<Blog?> GetById(int id)
        {
            return await _context.Blogs
                .Include(b => b.Posts.Where(p => p.PostState == PostState.Ready))
                .ThenInclude(p => p.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Blog?> FindById(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task Update(Blog entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var blog = await _context.Blogs
                    .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                    .FirstOrDefaultAsync(b => b.Id == id);

            _context.Remove(blog);
            await _context.SaveChangesAsync();

        }

    }

}
