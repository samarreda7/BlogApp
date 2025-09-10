using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class ShowCommentDTO
    {
        public int Id { get; set; }
        public string content { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string timestamp => CreatedAt.ToString("MMM dd, yyyy 'at' h:mm tt");

    }
}
