using BlogApp.Core.DTOs;
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
    }
}
