
namespace BlogApp.Core.Iservices
{
    public interface IAuthService
    {
        Task RegisterAsync(string username, string password);

    }
}