using BlogApp.Core;
using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using BlogApp.EF.Repository;
using Microsoft.EntityFrameworkCore;
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
        public async Task<Comment> AddCommentAsync(int postId, string userId, string content)
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

            var comment = await _unitOfWork.commentRepository.AddCommentAsync(commentDto, userId, postId);
            return comment;
        }


        public async Task<List<ShowCommentDTO>> GetCommentsAsync(int postId,string currentUserId)
        {
            
         return await Task.Run(() => _unitOfWork.commentRepository.ShowCommentsOfPost(postId,currentUserId));
        }
        public async Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds)
        {
            return await _unitOfWork.commentRepository.GetCommentCountsForPostsAsync(postIds);
        }
        public async Task<Comment> GetComment(int id)
        {
            return await _unitOfWork.commentRepository.GetComment(id);

        }

        public async Task<(bool Success, string ErrorMessage)> EditComment(int id, UpdateCommentDTO dto)
        {
            if (dto == null)
                return (false, "Update data cannot be null.");

            var comment = await _unitOfWork.commentRepository.GetComment(id);

            if (comment == null)
                return (false, "Comment not found.");

            string currentContent = comment.content?.Trim();
            string newContent = dto.Content?.Trim();

            if (currentContent == newContent)
            {
                return (false, "No changes detected.");
            }


            comment.content = dto.Content;
            comment.UpdatedAt = DateTime.Now;

            var saved = await _unitOfWork.commentRepository.UpdateComment(comment);

            if (!saved)
                return (false, "Failed to save changes to the database.");

            return (true, "Comment updated successfully.");

        }

        public async Task<(bool Success, string ErrorMessage)> DeleteComment(int id)
        {
            if (id <= 0)
                return (false, "Invalid comment ID.");

            try
            {
                _unitOfWork.commentRepository.DeleteComment(id);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "An error occurred while deleting the comment. Please try again.");
            }
        }


    }
}
