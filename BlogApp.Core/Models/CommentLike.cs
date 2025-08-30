using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Models
{
    [PrimaryKey(nameof(commentId), nameof(UserId))]
    public class CommentLike
    {
        public int commentId { get; set; }

        [ForeignKey("commentId")]
        public Comment comment { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
