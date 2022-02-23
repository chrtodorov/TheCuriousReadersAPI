using BusinessLayer.Models;
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    public static class UserMapper
    {
        public static UserEntity ToUserEntity(this User user, RoleEntity role)
        {
            return new UserEntity
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Role = role
            };
        }

        public static CustomerEntity ToCustomerEntity(this User customer, UserEntity user)
        {
            return new CustomerEntity
            {
                Address = customer.Address!.ToAddressEntity(),
                User = user,
            };
        }

        public static LibrarianEntity ToLibrarianEntity(this User librarian, UserEntity user)
        {
            return new LibrarianEntity
            {
                User = user,
            };
        }

        public static AdministratorEntity ToAdministartorEntity(this User admin, UserEntity user)
        {
            return new AdministratorEntity
            {
                User = user,
            };
        }
    }
}
