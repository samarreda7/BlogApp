using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class PostModelDTO
    {
        [Required(ErrorMessage = "Content is required.")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Write your post")]
        public string content { get; set; }

    }
}
