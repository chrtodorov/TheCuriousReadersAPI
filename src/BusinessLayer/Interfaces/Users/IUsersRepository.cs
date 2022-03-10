using BusinessLayer.Models;
using System.Security.Claims;

namespace BusinessLayer.Interfaces.Users
{
    public interface IUsersRepository
    {
        Task<User> GetUser(string email, string password, bool hashedPassword = false);
        Task<User> GetUser(string email);
        Task<int> GetCount();
        Task Register(User user);
        Task<User> ApproveUser(Guid userId, ClaimsPrincipal approver);
        Task RejectUser(Guid userId, ClaimsPrincipal rejecter);
        Task<IEnumerable<User>> GetPendingCustomers();
        Task<IEnumerable<User>> GetPendingUsers();

    }
}
