
using BlogApp.Core.DTOs;

namespace BlogApp.Core.Iservices
{
    public interface IAuthService
    {
        Task RegisterAsync(string username, string password);
        Task<LoginResult> LoginAsync(string username, string password);
    }
}