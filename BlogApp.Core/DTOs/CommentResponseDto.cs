using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string FirstName { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public bool IsEdited { get; set; }
        public bool IsAuthor { get; set; }
    }
}
