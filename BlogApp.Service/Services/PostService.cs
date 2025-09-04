using BlogApp.Core;
using BlogApp.Core.Iservices;
using BlogApp.Core.DTOs;
using BlogApp.Core.Models;


namespace BlogApp.Service.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            return await Task.Run(() => _unitOfWork.postRepository.GetMyPosts(userId));
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

        public async Task<Post> GetPostById(int Id)
        {
           return await _unitOfWork.postRepository.GetPost(Id);
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

    }

}