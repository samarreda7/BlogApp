using BlogApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public User User { get; set; }  
        public IEnumerable<Claim> Claims { get; set; }
        public string Error { get; set; }
    }
}
