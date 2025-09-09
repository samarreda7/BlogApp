using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.EF.Repository
{
    public class PostRepository : IPostRepository
    {
       AppDbContext _context;
        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreatePost(PostModelDTO model,string id)
        {
            Post post = new Post()
            {
                content = model.content,
                UserId = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            _context.Add(post);
            _context.SaveChanges();
        }
        public List<ShowPostsDTO> GetMyPosts(string id)
        {
            var posts = _context.Posts
                .Where(p => p.UserId == id)
                .Include(p => p.User)
                .Select(p=> new ShowPostsDTO
                { 
                    Id = p.Id,
                    FirstName=p.User.FirstName,
                    username=p.User.UserName,
                    CreatedAt=p.CreatedAt,
                    updateat=p.UpdatedAt,
                    Content=p.content,
                    IsLikedByCurrentUser = _context.PostLike.Any(l => l.PostId == p.Id && l.UserId == id)
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
                return posts;
           
        }
        public void DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
            else 
            {
                throw new Exception("there is no Post with this Id");
            }
        }
        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(p=>p.Id==id);

        }
        public async Task<bool> UpdatePost(Post post)
        {
          
            try
            {
              _context.Posts.Update(post);
              return await  _context.SaveChangesAsync() > 0;

            }
            catch
            {
               
                return false;
            }
        }
      
        public List<ShowPostsDTO> GetUserPosts(string username,string currentUserId)
        {

            var posts = _context.Posts.Where(p => p.User.UserName == username).Include(p => p.User)
                .Select(p => new ShowPostsDTO
                {
                    Id = p.Id,
                    FirstName = p.User.FirstName,
                    username = p.User.UserName,
                    CreatedAt = p.CreatedAt,
                    updateat = p.UpdatedAt,
                    Content = p.content,
                    IsLikedByCurrentUser = _context.PostLike.Any(l => l.PostId == p.Id && l.UserId == currentUserId)

                })
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
            return posts;

        }

    }
}
