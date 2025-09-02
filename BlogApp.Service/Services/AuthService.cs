using Microsoft.AspNetCore.Identity;
using BlogApp.Core.Iservices;
using BlogApp.Core.Models;
using BlogApp.EF;
using BlogApp.Core;
using BlogApp.Core.DTOs;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BlogApp.Service.Services
{
    public class AuthService : IAuthService
    {
  
        IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<User> _signInManager;
        public AuthService(
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger,
            SignInManager<User> signInManager
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _signInManager = signInManager;
        }



        public async Task RegisterAsync(RegisterModel model)
        {
            if (string.IsNullOrWhiteSpace(model.username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(model.username));
            if (string.IsNullOrWhiteSpace(model.password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(model.password));

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.username,
                createdAt = DateTime.UtcNow,
                updateddAt = DateTime.UtcNow
            };

            await CreateUserAndAssignRoleAsync(user, model.password, "User");
        }

        private async Task CreateUserAndAssignRoleAsync(User user, string password, string role)
        {
            var createResult = await _unitOfWork.userRepository.CreateUserAsync(user, password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"User creation failed: {errors}");
            }

            var roleResult = await _unitOfWork.userRepository.AddUserToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Role assignment failed: {errors}");
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
                new Claim("Firstname", user.FirstName),
                new Claim("Lastname", user.LastName),
                new Claim("uid", user.Id)

            };
                await _signInManager.SignInAsync(user, isPersistent: false); 
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

