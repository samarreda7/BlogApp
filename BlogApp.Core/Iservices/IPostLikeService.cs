using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Iservices
{
    public interface IPostLikeService
    {
        Task<(bool liked, int likeCount)> ToggleLikeAsync(int postId, string userId);
        Task<Dictionary<int, int>> GetLikeCountsForPostsAsync(List<int> postIds);
        Task<Dictionary<int, bool>> GetUserLikeStatusForPostsAsync(List<int> postIds, string userId);
    }
}
