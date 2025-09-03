using BlogApp.Core.DTOs;
using BlogApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.IRepository
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<IdentityResult> AddUserToRoleAsync(User user, string role);
        Task<User> GetUserByNameAsync(string username);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task<User> GetUserByIdAsync(string Id);
    }
}
