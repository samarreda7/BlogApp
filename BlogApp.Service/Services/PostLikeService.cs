using BlogApp.Core;
using BlogApp.Core.Iservices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Services
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostLikeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(bool liked, int likeCount)> ToggleLikeAsync(int postId, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required.", nameof(userId));

            var post = await _unitOfWork.postRepository.GetPost(postId);
            if (post == null)
                throw new ArgumentException("Post not found.", nameof(postId));

            var isLikedNow = await _unitOfWork.postLikeRepository.ToggleLikeAsync(postId, userId);

            var newLikeCount = await _unitOfWork.postLikeRepository.GetLikeCountAsync(postId);

            return (isLikedNow, newLikeCount);
        }

        public async Task<Dictionary<int, int>> GetLikeCountsForPostsAsync(List<int> postIds)
        {
            return await _unitOfWork.postLikeRepository.GetLikeCountsForPostsAsync(postIds);
        }

        public async Task<Dictionary<int, bool>> GetUserLikeStatusForPostsAsync(List<int> postIds, string userId)
        {
            return await _unitOfWork.postLikeRepository.GetUserLikeStatusForPostsAsync(postIds, userId);
        }
    }
}
