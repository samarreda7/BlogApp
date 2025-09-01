using BlogApp.Core;
using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using BlogApp.EF.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
 

namespace BlogApp.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public IUserRepository userRepository {  get; private set; }

        public UnitOfWork(AppDbContext context, UserManager<User> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager) 
        {
            _context = context;
            _config = config;
            userRepository = new UserRepository(userManager);
        }
    }
}
