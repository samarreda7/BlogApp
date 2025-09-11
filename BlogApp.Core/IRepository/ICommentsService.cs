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
        Task AddCommentAsync(int postId, string userId, string content);
        Task<List<ShowCommentDTO>> GetCommentsAsync(int postId, string currentUserId);
        Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds);
        Task<Comment> GetComment(int id);
        Task<(bool Success, string ErrorMessage)> EditComment(int id, UpdateCommentDTO dto);
    }
}
