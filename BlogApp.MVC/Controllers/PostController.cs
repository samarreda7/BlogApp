using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using BlogApp.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using System.Net;
using System.Security.Claims;

namespace BlogApp.MVC.Controllers
{
    public class PostController : Controller
    {

        IPostService _postService;
        IPostLikeService _postLikeService;
        ICommentsService _commentsService;
        private readonly UserManager<User> _userManager;
        
        public PostController(IPostService postService,
            UserManager<User> userManager,
            IPostLikeService postLikeService,
            ICommentsService commentsService
            )
        {
            _postService = postService;
            _userManager = userManager;
            _postLikeService = postLikeService;
            _commentsService = commentsService;
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
            var userId = _userManager.GetUserId(User);
            var posts = await _postService.GetPostsByUserIdAsync(username,userId);

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

        [HttpPost]
        [Authorize]
        [ActionName("ToggleLike")]
        public async Task<IActionResult> ToggleLikeAction([FromBody]ToggleLikeDto togglelike)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "User not logged in" });

                var (liked, likeCount) = await _postLikeService.ToggleLikeAsync(togglelike.PostId, userId);
                return Json(new { liked, likeCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
       

        [HttpGet]
        public async Task<IActionResult> GetComments(int postId)
        {
            if (postId <= 0)
                return BadRequest(new { message = "Invalid post ID." });

            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var comments = await _commentsService.GetCommentsAsync(postId, currentUserId);
                return Json(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to load comments: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequestDto request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request?.content))
            {
                return BadRequest(); 
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                await _commentsService.AddCommentAsync(request.PostId, userId, request.content);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to post comment.");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment([FromBody] UpdateCommentDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message) = await _commentsService.EditComment(dto.Id, dto);

            if (!success)
            {
                return BadRequest(new { message });
            }

            // Return updated comment data for UI refresh
            var updatedComment = await _commentsService.GetComment(dto.Id); 

            if (updatedComment == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                success = true,
                comment = new
                {
                    id = updatedComment.Id,
                    content = updatedComment.content,
                    updatedAt = updatedComment.UpdatedAt.ToString("MMM dd, yyyy 'at' h:mm tt"),
                    isEdited = (updatedComment.UpdatedAt - updatedComment.CreatedAt).TotalMinutes > 1
                }
            });
        }
    }
}
