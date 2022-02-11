using BusinessLayer.Models.Requests;

namespace BusinessLayer.Interfaces.Users
{
    public interface IUsersRepository
    {
        string Authenticate(AuthenticateRequest authenticateRequest);
    }
}
