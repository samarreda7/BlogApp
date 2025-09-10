using BlogApp.Core.DTOs;
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
        Task<List<ShowCommentDTO>> GetCommentsAsync(int postId);
        Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds);
    }
}
