using BlogApp.Core.DTOs;
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using System.Net;

namespace BlogApp.MVC.Controllers
{
    public class PostController : Controller
    {

        IPostService _postService;
        private readonly UserManager<User> _userManager;
        public PostController(IPostService postService, UserManager<User> userManager)
        {
            _postService = postService;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostModelDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var (Success, ErrorMessage) = await _postService.CreatePostAsync(model, user.Id);
            

            if (!Success)
            {
                ModelState.AddModelError("", ErrorMessage);
                return View(model);
            }
            TempData["SuccessMessage"] = "Your post has been published!";
            return RedirectToAction("MyPosts", "Post");
        }

        [Authorize]
        public async Task<IActionResult> MyPosts()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            var posts = await _postService.GetMyPostsAsync(userId);
            return View(posts);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid post ID.");
            }

            await _postService.DeletePost(id);

            TempData["Message"] = "Post deleted successfully.";
            return RedirectToAction("MyPosts", "Post");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _postService.GetPostForEditAsync(id);
            if (model == null)
                return NotFound();

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePostDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (Success, Message) = await _postService.EditPost(model.Id, model);

            if (!Success)
            {
                ModelState.AddModelError("", Message);
                return View(model);
            }

            TempData["Message"] = Message;
            return RedirectToAction("MyPosts");
        }

        [HttpGet]
        [Route("Post/Profile/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserProfile(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var posts = await _postService.GetPostsByUserIdAsync(username);

            if (!posts.Any())
            {
                ViewBag.FirstName = "User";
                ViewBag.username = username;
            }
            else
            {
                ViewBag.FirstName = posts[0].FirstName;
                ViewBag.username = posts[0].username;
            }

            return View("UserPosts", posts); 
        }
    }
}
