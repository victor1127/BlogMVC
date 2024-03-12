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

namespace BlogMVC.Controllers
{
    [Authorize]
    public class BlogsController : Controller
    {
        private readonly IBlogRepository<Blog> blogContext;
        private readonly ImageService imageService;

        public BlogsController(IBlogRepository<Blog> blogRepository, ImageService imageService)
        {
            blogContext = blogRepository;
            this.imageService = imageService;
        }




        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var blogs = await blogContext.GetAll();
            return View(blogs);
        }



        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(blog);
        }



        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,ImageFile")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.Created = DateTime.Now;

                if(blog.ImageFile != null)
                {
                    blog.ContentType = blog.ImageFile?.ContentType;
                    blog.ImageData = imageService.ConvertFileToByte(blog.ImageFile);
                }

                await blogContext.Add(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }



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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,ImageData")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await blogContext.Update(blog);
                return RedirectToAction(nameof(Index));
            }

            return View(blog);
        }



        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await blogContext.FindById((int)id);
            if (blog == null) return NotFound();

            return View(blog);
        }



        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await blogContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        private bool BlogExists(int id)
        {
            return true;
        }
    }
}
