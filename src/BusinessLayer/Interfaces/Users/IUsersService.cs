using BusinessLayer.Models;
using BusinessLayer.Models.Requests;

namespace BusinessLayer.Interfaces.Users
{
    public interface IUsersService
    {
        Task<AuthenticatedUser> Authenticate(string email, string password);

        Task<AuthenticatedUser> Register(User user);
    }
}
