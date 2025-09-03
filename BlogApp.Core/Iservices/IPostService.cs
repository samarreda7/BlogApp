using BlogApp.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Iservices
{
    public interface IPostService
    {
        Task<(bool Success, string ErrorMessage)> CreatePostAsync(PostModelDTO model, string userId);
        Task<List<ShowPostsDTO>> GetMyPostsAsync(string userId);
        Task<(bool Success, string ErrorMessage)> DeletePost(int Id);
    }
}
