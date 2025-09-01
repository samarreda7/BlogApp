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

        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User> GetUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<bool> ValidatePasswordAsync(User user, string password)
        {
            if (user == null) return false;
            return await _userManager.CheckPasswordAsync(user, password);
        }


    }
}
