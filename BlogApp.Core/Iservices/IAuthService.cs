using BlogApp.Core.DTOs;

namespace BlogApp.Core.Iservices
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterModel model);
        Task<LoginResult> LoginAsync(string username, string password);
         void LogOut();
    }
}