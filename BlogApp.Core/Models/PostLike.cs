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
    [PrimaryKey(nameof(PostId), nameof(UserId))]
    public class PostLike
    {

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post post { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
