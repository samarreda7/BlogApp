using BlogApp.Core;
using BlogApp.Core.Iservices;
using BlogApp.Core.DTOs;


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

    }

}