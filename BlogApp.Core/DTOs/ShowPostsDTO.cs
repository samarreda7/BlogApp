using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class ShowPostsDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string username { get; set; }
        public DateTime CreatedAt  { get; set; }
        public string Content { get; set; }
    }
}
