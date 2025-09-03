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
    }
}
