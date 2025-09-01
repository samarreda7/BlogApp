using Microsoft.AspNetCore.Identity;

namespace BlogApp.Core.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updateddAt { get; set; }

    }
}