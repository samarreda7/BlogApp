using BlogApp.Core.DTOs;
using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface ICommentRepository
    {
        Task<Comment> AddCommentAsync(AddCommentDTO commentDto, string userId, int postId);
        List<ShowCommentDTO> ShowCommentsOfPost(int postId, string currentUserId);
        Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds);
        Task<Comment> GetComment(int id);
        Task<bool> UpdateComment(Comment comment);
        Task<bool> DeleteComment(int id);
    }
}
