using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Data;
using BlogMVC.Models;
using BlogMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using BlogMVC.Services;
using Microsoft.AspNetCore.Identity;
using BlogMVC.Helpers;
using Microsoft.AspNetCore.Diagnostics;

namespace BlogMVC.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogRepository<Blog> blogContext;
        private readonly ImageService imageService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<BlogUser> userManager;
        private readonly IConfiguration _configuration;


        public BlogsController(IBlogRepository<Blog> blogRepository,
                                ImageService imageService,
                                UserManager<BlogUser> manager,
                                IConfiguration configuration)
        {
            blogContext = blogRepository;
            this.imageService = imageService;
            userManager = manager;
            _configuration = configuration;
        }




        // GET: Blogs
        public async Task<IActionResult> Index(int? pageIndex)
        {
            var blogs = await blogContext.GetAll();
            blogs?.OrderByDescending(b => b.Created);

            pageIndex = pageIndex ?? 1;

            var paginatedBlogs = PaginatedList<Blog>.CreateAsync(blogs, (int)pageIndex, int.Parse(_configuration["PageSize"]));
            return View(paginatedBlogs);
        }

        public async Task<IActionResult> SearchIndex(int? pageIndex, string searchInput)
        {           
            var blogs = await blogContext.GetAll();

            pageIndex = pageIndex ?? 1;

            if (searchInput != null)
            {
                searchInput = searchInput.ToLower();

                blogs = blogs?.Where(
                    b => b.Name.ToLower().Contains(searchInput) ||
                    b.Description.ToLower().Contains(searchInput));
            }

            ViewData["searchInput"] = searchInput;
            blogs = blogs?.OrderByDescending(b => b.Created);
            var paginatedBlogs = PaginatedList<Blog>.CreateAsync(blogs, (int)pageIndex, int.Parse(_configuration["PageSize"]));
            
            return View(paginatedBlogs);
        }


        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await blogContext.GetById((int)id);
            if (blog == null)
            {
                return NotFound();
            }

            blog.Posts?.OrderByDescending(p => p.Created);
            pageIndex = pageIndex ?? 1;
            ViewData["BlogId"] = blog?.Id;

            var paginatedBlogs = PaginatedList<Post>.CreateAsync(blog.Posts, (int)pageIndex, int.Parse(_configuration["PageSize"]));
            return View(paginatedBlogs);

        }


        [Authorize]
        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Blog blog)
        {
            ModelState.Remove("AuthorId");
            ModelState.Remove("Author");

            if (ModelState.IsValid)
            {
                blog.AuthorId = userManager.GetUserId(User);
                blog.Created = DateTime.Now;

                if(blog.ImageFile != null)
                {
                    blog.ContentType = blog.ImageFile?.ContentType;
                    blog.ImageData = await imageService.EncodeImageAsync(blog.ImageFile);
                }

                await blogContext.Add(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }


        [Authorize]
        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog =  await blogContext.FindById((int)id);
            if (blog == null) return NotFound();

            return View(blog);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,ImageFile")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (blog.ImageFile != null)
                {
                    blog.ContentType = blog.ImageFile?.ContentType;
                    blog.ImageData = await imageService.EncodeImageAsync(blog.ImageFile);
                }

                blog.Updated = DateTime.Now;

                await blogContext.Update(blog);
                return RedirectToAction(nameof(Index));
            }

            return View(blog);
        }



        // GET: Blogs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await blogContext.FindById((int)id);
            if (blog == null) return NotFound();

            return View(blog);
        }



        // POST: Blogs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await blogContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool BlogExists(int id)
        {
            return true;
        }
    }
}
