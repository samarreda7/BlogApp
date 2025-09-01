
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using BlogApp.EF;
using Microsoft.AspNetCore.Identity;
using BlogApp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Core;
using BlogApp.Core.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BlogApp.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _unitOfWork = unitOfWork;
            _logger = logger;
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


        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new LoginResult { Success = false };
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new LoginResult { Success = false };
            }

            try
            {
                var user = await _unitOfWork.userRepository.GetUserByNameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: User '{Username}' not found.", username);
                    return new LoginResult { Success = false };
                }

                var isValid = await _unitOfWork.userRepository.ValidatePasswordAsync(user, password);
                if (!isValid)
                {
                    _logger.LogWarning("Login failed: Invalid password for user '{Username}'.", username);
                    return new LoginResult { Success = false };
                }

                var claims = new List<Claim>
            {
                new Claim("email", user.Email ?? user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("uid", user.Id)
            };

                return new LoginResult
                {
                    Success = true,
                    User = user,
                    Claims = claims
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user '{Username}'", username);
                return new LoginResult
                {
                    Success = false
                };
            }
        }
    }
}

