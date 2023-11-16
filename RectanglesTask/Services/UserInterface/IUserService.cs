using RectanglesTask.Models;

namespace RectanglesTask.Services.UserInterface
{
    public interface IUserService
    {
        Task<ApplicationUser> AuthenticateAsync(string username, string password);
    }
}
