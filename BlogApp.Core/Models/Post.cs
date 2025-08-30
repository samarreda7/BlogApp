using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string content { get; set; }

        public string UserId { get; set; }   

        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
