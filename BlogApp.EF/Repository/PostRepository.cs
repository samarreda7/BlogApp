using BlogApp.Core.DTOs;
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
            var posts = _context.Posts.Where(p => p.UserId == id).Include(p => p.User)
                .Select(p=> new ShowPostsDTO
                { 
                    FirstName=p.User.FirstName,
                    username=p.User.UserName,
                    CreatedAt=p.CreatedAt,
                    Content=p.content

                }).ToList();
            return posts;
           
        }
    }
}
