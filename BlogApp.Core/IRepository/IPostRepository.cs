using BlogApp.Core.DTOs;
using BlogApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface IPostRepository
    {
        void CreatePost(PostModelDTO model, string id);
        List<ShowPostsDTO> GetMyPosts(string id);
        void DeletePost(int id);
        Task<Post> GetPost(int id);
        Task<bool> UpdatePost(Post post);
     
    }
}
