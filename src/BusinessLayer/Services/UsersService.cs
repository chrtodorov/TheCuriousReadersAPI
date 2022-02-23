using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;

namespace BusinessLayer
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<AuthenticatedUser> Authenticate(string email, string password) =>
            await usersRepository.Authenticate(email, password);

        public async Task<AuthenticatedUser> Register(User user) => 
            await usersRepository.Register(user);
    }
}
