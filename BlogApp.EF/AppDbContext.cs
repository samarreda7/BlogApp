using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlogApp.Core.Models;


namespace BlogApp.EF
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }    
        public DbSet<PostLike> PostLike { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    }
}
