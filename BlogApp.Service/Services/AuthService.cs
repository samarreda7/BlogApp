
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using BlogApp.EF;
using Microsoft.AspNetCore.Identity;
using BlogApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Service.Services





{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task RegisterAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            string defaultRole = "User";

            var user = new User
            {
                UserName = username,
                createdAt = DateTime.UtcNow,
                updateddAt = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(user, defaultRole);
            if (!addToRoleResult.Succeeded)
            {
                var roleErrors = string.Join("; ", addToRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to assign role '{defaultRole}' to user '{user.UserName}': {roleErrors}");
            }
        }
    }
}