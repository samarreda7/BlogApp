using BlogApp.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface ICommentRepository
    {
        void AddComment(AddCommentDTO commentDto, string UserId, int postId);
        List<ShowCommentDTO> ShowCommentsOfPost(int postId);
        Task<Dictionary<int, int>> GetCommentCountsForPostsAsync(List<int> postIds);
    }
}
