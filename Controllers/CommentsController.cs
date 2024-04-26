using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Data;
using BlogMVC.Models;
using Microsoft.AspNetCore.Identity;
using BlogMVC.Repositories;

namespace BlogMVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogRepository<Comment> _commentContext;
        private readonly UserManager<BlogUser> userManager;


        public CommentsController(ApplicationDbContext context, UserManager<BlogUser> userManager, IBlogRepository<Comment> commentContext)
        {
            _context = context;
            this.userManager = userManager;
            _commentContext = commentContext;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.Author).Include(c => c.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Content");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId, Body")] Comment comment)
        {
            ModelState.Remove("Post");
            ModelState.Remove("Author");
            ModelState.Remove("AuthorId");

            if (ModelState.IsValid)
            {
                comment.AuthorId = userManager.GetUserId(User);
                comment.Created = DateTime.Now;

                _commentContext.Add(comment);
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostId });
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", comment.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "Content", comment.PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body,PostId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Comment.Post");
            ModelState.Remove("Comment.Author");
            ModelState.Remove("Comment.AuthorId");

            if (ModelState.IsValid)
            {    
                try
                {
                    comment.Updated = DateTime.Now;
                    var newComment = await _commentContext.GetById(comment.Id);
                    newComment.Body = comment.Body;
                    await _commentContext.Update(newComment);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("Details", "Posts", new {id = comment.PostId}, "commentSection");

        }

        public async Task<IActionResult> Moderate(int id, [Bind("Id, PostId, ModeratedReason")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Comment.Post");
            ModelState.Remove("Comment.Author");
            ModelState.Remove("Comment.AuthorId");
            ModelState.Remove("Comment.Body");


            if (ModelState.IsValid)
            {
                comment.Updated = DateTime.Now;
                var newComment = await _commentContext.GetById(comment.Id);
                newComment.ModeratedReason = comment.ModeratedReason;
                await _commentContext.Update(newComment);
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostId }, "commentSection");

        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _commentContext.FindById((int)id);
            if (comment != null)
            {
                _commentContext.Delete(comment.Id);
            }

            return RedirectToAction("Details", "Posts", new {id = comment.PostId}, "commentSection");
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
