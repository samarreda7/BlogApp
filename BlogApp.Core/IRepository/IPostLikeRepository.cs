using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface IPostLikeRepository
    {
        Task<int> GetLikeCountAsync(int postId);
        Task<Dictionary<int, int>> GetLikeCountsForPostsAsync(List<int> postIds);
        Task<Dictionary<int, bool>> GetUserLikeStatusForPostsAsync(List<int> postIds, string userId);
        Task<bool> ToggleLikeAsync(int postId, string userId);
    }
}
