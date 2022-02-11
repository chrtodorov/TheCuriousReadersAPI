using BusinessLayer.Models.Requests;

namespace BusinessLayer.Interfaces.Users
{
    public interface IUsersService
    {
        string Authenticate(AuthenticateRequest authenticateRequest);
    }
}
