using BlogApp.Core.DTOs;
using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface ICommentsService
    {
        Task<Comment> AddCommentAsync(int postId, string userId, string content);
        List<ShowCommentDTO> GetComments(int postId, string currentUserId);
        Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds);
        Task<Comment> GetComment(int id);
        Task<(bool Success, string ErrorMessage)> EditComment(int id, UpdateCommentDTO dto);
        Task<(bool Success, string ErrorMessage)> DeleteComment(int Id);
        Task<CommentResponseDto> GetCommentResponseAsync(int commentid, string currentUserId);
    }
}
