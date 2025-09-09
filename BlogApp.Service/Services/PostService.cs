using BlogApp.Core;
using BlogApp.Core.Iservices;
using BlogApp.Core.DTOs;
using BlogApp.Core.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace BlogApp.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
       private readonly IPostLikeService _postLikeService;
        public PostService(IUnitOfWork unitOfWork, 
            IHttpContextAccessor httpContextAccessor,
              IPostLikeService postLikeService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _postLikeService = postLikeService;
        }

        public async Task<(bool Success, string ErrorMessage)> CreatePostAsync(PostModelDTO model, string userId)
        {

            if (model == null)
                return (false, "Post data is null.");


            if (string.IsNullOrWhiteSpace(model.content))
                return (false, "Content cannot be empty.");


            var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            try
            {
                _unitOfWork.postRepository.CreatePost(model, userId);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "An error occurred while saving the post. Please try again.");
            }
        }

        public async Task<List<ShowPostsDTO>> GetMyPostsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

           
            var posts = await Task.Run(() => _unitOfWork.postRepository.GetMyPosts(userId));

            if (!posts.Any()) return posts;

            var postIds = posts.Select(p => p.Id).ToList();

           
            var likeCounts = await _postLikeService.GetLikeCountsForPostsAsync(postIds);

            
            var userLikeStatus = await _postLikeService.GetUserLikeStatusForPostsAsync(postIds, userId);

           
            foreach (var post in posts)
            {
                post.LikeCount = likeCounts.GetValueOrDefault(post.Id, 0);
                post.IsLikedByCurrentUser = userLikeStatus.GetValueOrDefault(post.Id, false);
            }

            return posts;

        }


        public async Task<UpdatePostDTO> GetPostForEditAsync(int id)
        {
            var post = await _unitOfWork.postRepository.GetPost(id);
            if (post == null) return null;

            return new UpdatePostDTO
            {
                Id = post.Id,
                Content = post.content,
            };
        }


        public async Task<(bool Success, string ErrorMessage)> EditPost(int id, UpdatePostDTO dto)
        {
            if (dto == null)
                return (false, "Update data cannot be null.");

            var post = await _unitOfWork.postRepository.GetPost(id);

            if (post == null)
                return (false, "Post not found.");

            string currentContent =  post.content?.Trim();
            string newContent = dto.Content?.Trim();

            if (currentContent == newContent)
            {
                return (false, "No changes detected.");
            }

            
            post.content = dto.Content;
            post.UpdatedAt = DateTime.Now;

            var saved = await  _unitOfWork.postRepository.UpdatePost(post); 

            if (!saved)
                return (false, "Failed to save changes to the database.");

            return (true, "Post updated successfully.");
        
        }

        public async Task<List<ShowPostsDTO>> GetPostsByUserIdAsync(string username , string currentUserId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            var posts = _unitOfWork.postRepository.GetUserPosts(username, currentUserId);

            if (!posts.Any()) return posts;

            var postIds = posts.Select(p => p.Id).ToList();

           
            var likeCounts = await _postLikeService.GetLikeCountsForPostsAsync(postIds);

        
            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            
            var userLikeStatus = new Dictionary<int, bool>();
            if (!string.IsNullOrEmpty(currentUserId))
            {
                userLikeStatus = await _postLikeService.GetUserLikeStatusForPostsAsync(postIds, currentUserId);
            }

            
            foreach (var post in posts)
            {
                post.LikeCount = likeCounts.GetValueOrDefault(post.Id, 0);
                post.IsLikedByCurrentUser = userLikeStatus.GetValueOrDefault(post.Id, false);
            }

            return posts;
        }

        public async Task<(bool Success, string ErrorMessage)> DeletePost(int Id)
        {
            if (Id == null)
            {
                return (false, "Post Id is null.");
            }
            try
            {
                _unitOfWork.postRepository.DeletePost(Id);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "An error occurred while saving the post. Please try again.");
            }
        }
    }

}