using BusinessLayer.Models;
using BusinessLayer.Requests;
using BusinessLayer.Responses;
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

        public static User ToUser(this UserEntity userEntity)
        {
            return new User
            {
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                EmailAddress = userEntity.EmailAddress,
                Password = userEntity.Password,
                PhoneNumber = userEntity.PhoneNumber,
                RoleName = userEntity.Role.Name,
                Status = userEntity.Status,
            };
        }

        public static User ToUser(this UserRequest userRequest)
        {
            return new User
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                EmailAddress = userRequest.EmailAddress,
                PhoneNumber = userRequest.PhoneNumber,
                RoleName = userRequest.RoleName,
                Address = userRequest.Address,
                Password = userRequest.Password,
            };
        }

        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                RoleName = user.RoleName,
                Status = user.Status
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
