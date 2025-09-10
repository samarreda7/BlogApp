using BlogApp.Core;
using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.EF.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.Services
{
    public class CommentService :ICommentsService
    {
        IUnitOfWork _unitOfWork;
        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddCommentAsync(int postId, string userId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content cannot be empty.", nameof(content));

            var commentDto = new AddCommentDTO
            {
                PostId = postId,
                content = content.Trim(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

             _unitOfWork.commentRepository.AddComment(commentDto, userId, postId);

        }

        public async Task<List<ShowCommentDTO>> GetCommentsAsync(int postId)
        {
            
            return await Task.Run(() => _unitOfWork.commentRepository.ShowCommentsOfPost(postId));
        }
        public async Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds)
        {
            return await _unitOfWork.commentRepository.GetCommentCountsForPostsAsync(postIds);
        }
    }
}
