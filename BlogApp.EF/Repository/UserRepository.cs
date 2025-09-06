using BlogApp.Core.DTOs;
using BlogApp.Core.IRepository;
using BlogApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.EF.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        AppDbContext _context;
        public UserRepository(AppDbContext Context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = Context;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
  

        public async Task<IdentityResult> AddUserToRoleAsync(User user, string role)
        {
           return await _userManager.AddToRoleAsync(user, role);
        }
          

        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<User> GetUserByIdAsync(string Id)
        {
            return await _context.Users.FindAsync(Id);
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            if (user == null) return false;
            return await _userManager.CheckPasswordAsync(user, password);
        }

        // UserRepository.cs
        public async Task<List<UserSearchDTO>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<UserSearchDTO>();

            return await _context.Users
                .Where(u => u.FirstName.Contains(query) ||
                            u.UserName.Contains(query))
                .Select(u => new UserSearchDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    UserName = u.UserName
                })
                .Take(10)
                .ToListAsync();
        }


    }
}
