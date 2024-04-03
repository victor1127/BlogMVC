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
using BlogMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlogMVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogRepository<Post> _context;
        private readonly IBlogRepository<Tag> _tagContext;
        private readonly IBlogRepository<Blog> _blogContext;


        private readonly ImageService imageService;
        private readonly UserManager<BlogUser> userManager;

        public PostsController(ApplicationDbContext context, IBlogRepository<Post> postContext, ImageService imageService, UserManager<BlogUser> userManager, IBlogRepository<Tag> tagContext, IBlogRepository<Blog> blogContext)
        {
            this._context = postContext;
            this.imageService = imageService;
            this.userManager = userManager;
            _tagContext = tagContext;
            _blogContext = blogContext;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.GetAll();
            return View(posts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.GetById((int)id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["BlogId"] = new SelectList(await _blogContext.GetAll(), "Id", "AuthorId");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Title,Preview,Content,PostState,Slug,ImageFile")] Post post, List<string> tags)
        {
            if (ModelState.IsValid)
            {
                var authorId = userManager.GetUserId(User);
                post.Created = DateTime.Now;
                post.AuthorId = authorId;

                if (post.ImageFile != null)
                {
                    post.ContentType = post.ImageFile?.ContentType;
                    post.ImageData = await imageService.EncodeImageAsync(post.ImageFile);
                }

                await _context.Add(post);

                foreach(var tagText in tags)
                {
                    var tag = new Tag
                    {
                        AuthorId = authorId,
                        PostId = post.Id,
                        Content = tagText
                    };

                    await _tagContext.Add(tag);
                }
              
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogId"] = new SelectList(await _blogContext.GetAll(), "Id", "AuthorId", post.Id);
            ViewData["TagValues"] = string.Join(",", tags);

            return View(post);
        }




        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.GetById((int)id);
            if (post == null)
            {
                return NotFound();
            }

            ViewData["BlogId"] = new SelectList(await _blogContext.GetAll(), "Id", "AuthorId", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Title,Preview,Content,PostState, ImageFile")] Post post, List<string> tags)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                try
                {
                    var currentPost = await _context.GetById(post.Id);    
                    
                    if (post.ImageFile != null)
                    {
                        post.ContentType = post.ImageFile?.ContentType;
                        post.ImageData = await imageService.EncodeImageAsync(post.ImageFile);
                    }
                    else
                    {
                        post.ImageData = currentPost.ImageData;
                    }

                    currentPost = post;
                    currentPost.Tags = null;
                    //_context.tags.removeRange(post.tags)

                    await _context.Update(currentPost);

                    foreach (var tagText in tags)
                    {
                        var tag = new Tag
                        {
                            AuthorId = post.AuthorId,
                            PostId = post.Id,
                            Content = tagText
                        };

                        await _tagContext.Add(tag);
                    }

                    post.Updated = DateTime.Now;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["BlogId"] = new SelectList(await _blogContext.GetAll(), "Id", "AuthorId", post.BlogId);
            return View(post);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.FindById((int)id);
                
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.GetById(id);
            if (post != null)
            {
                await _context.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return true;

        }
    }
}
