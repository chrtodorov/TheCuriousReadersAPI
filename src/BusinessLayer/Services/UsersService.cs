using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models.Requests;

namespace BusinessLayer
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string Authenticate(AuthenticateRequest authenticateRequest)
        {
            return usersRepository.Authenticate(authenticateRequest);
        }
    }
}
