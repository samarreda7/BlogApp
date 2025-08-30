using Microsoft.AspNetCore.Identity;

namespace BlogApp.Core.Models
{
    public class User : IdentityUser
    {
        public DateTime createdAt { get; set; }
        public DateTime updateddAt { get; set; }

    }
}