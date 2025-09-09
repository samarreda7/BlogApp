using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.EF.Repository
{
    public class PostLikeRepository : IPostLikeRepository
    {
        AppDbContext _context;
        public PostLikeRepository(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<int> GetLikeCountAsync(int postId)
        {
            return await _context.PostLike.CountAsync(pl => pl.PostId == postId);
        }
        public async Task<Dictionary<int, int>> GetLikeCountsForPostsAsync(List<int> postIds)
        {
            return await _context.PostLike
                .Where(pl => postIds.Contains(pl.PostId))
                .GroupBy(pl => pl.PostId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }
        public async Task<Dictionary<int, bool>> GetUserLikeStatusForPostsAsync(List<int> postIds, string userId)
        {
            var likedPostIds = await _context.PostLike
                .Where(pl => postIds.Contains(pl.PostId) && pl.UserId == userId)
                .Select(pl => pl.PostId)
                .ToListAsync();

            return postIds.ToDictionary(id => id, id => likedPostIds.Contains(id));
        }
        public async Task<bool> ToggleLikeAsync(int postId, string userId)
        {
            var existing = await _context.PostLike
         .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);

            if (existing != null)
            {
                _context.PostLike.Remove(existing);
                await _context.SaveChangesAsync();
                return false; 
            }
            else
            {
                var newLike = new PostLike
                {
                    PostId = postId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.PostLike.Add(newLike);
                await _context.SaveChangesAsync();
                return true; 
            }
        }
    }
}
