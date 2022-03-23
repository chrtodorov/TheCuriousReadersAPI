using BusinessLayer.Enumerations;
using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Mappers;
using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccessTests
{
    public class UsersRepositoryTests
    {
        private IUsersRepository _usersRepository = null!;
        private ILogger<UsersRepository> _logger = null!;
        private IConfiguration _configuration = null!;
        private DataContext _context = null!;
        private User validAdmin = new User
        {
            FirstName = "Valid",
            LastName = "Admin",
            EmailAddress = "admin@abv.bg",
            Password = "1234",
            RoleName = "administrator",
            PhoneNumber = "123456789"
        };

        private User validLibrarian = new User
        {
            FirstName = "Valid",
            LastName = "Librarian",
            EmailAddress = "librarian@abv.bg",
            Password = "1234",
            RoleName = "librarian",
            PhoneNumber = "123456789"
        };

        private User validCustomer = new User
        {
            FirstName = "Valid",
            LastName = "Customer",
            EmailAddress = "customer@abv.bg",
            Password = "1234",
            RoleName = "customer",
            PhoneNumber = "123456789",
            Address = new Address
            {
                Country = "Bulgaria",
                City = "Plovdiv",
                Street = "main street",
                ApartmentNumber = "14",
                BuildingNumber = "1",
                StreetNumber = "15",
                AdditionalInfo = "test info"
            }
        };

        private User invalidUser = new User
        {
            FirstName = "Invalid",
            LastName = "User",
            EmailAddress = "invalid@abv.bg",
            Password = "1234",
            RoleName = "not valid",
            PhoneNumber = "123456789"
        };

        [SetUp]
        public async Task Setup()
        {
            _logger = Substitute.For<ILogger<UsersRepository>>();
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _usersRepository = new UsersRepository(_context, _logger);
            }
            await RoleSeeder.SeedRolesAsync(_context);
        }

        [Test]
        public async Task Register()
        {
            await _usersRepository.Register(validLibrarian);
            var librarianExists = await _context.Librarians.AnyAsync();
            Assert.That(librarianExists);

            await _usersRepository.Register(validCustomer);
            var customerExists = await _context.Customers.AnyAsync();
            Assert.That(customerExists);

            await _usersRepository.Register(validAdmin);
            var adminExists = await _context.Administrators.AnyAsync();
            Assert.That(adminExists);
        }

        [Test]
        public void Register_Fails()
        {
            Assert.ThrowsAsync<ArgumentException>(async delegate { await _usersRepository.Register(invalidUser); });
        }

        [Test]
        public async Task GetUser()
        {
            var librarianRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == validLibrarian.RoleName);
            var userEntity = validLibrarian.ToUserEntity(librarianRole!);
            userEntity.Status = AccountStatus.Approved;
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            var user = await _usersRepository.GetUser(validLibrarian.EmailAddress, validLibrarian.Password);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.FirstName, Is.Not.Null.Or.Empty);
            Assert.That(user.LastName, Is.Not.Null.Or.Empty);
            Assert.That(user.PhoneNumber, Is.Not.Null.Or.Empty);
            Assert.That(user.EmailAddress, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task GetUser_Fails_NotApproved()
        {
            await _usersRepository.Register(validCustomer);
            Assert.ThrowsAsync<ArgumentException>(async delegate 
            { 
                await _usersRepository.GetUser(validLibrarian.EmailAddress, validLibrarian.Password); 
            });
        }

        [Test]
        public async Task GetUser_Fails_InvalidPassword()
        {
            var librarianRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == validLibrarian.RoleName);
            var userEntity = validLibrarian.ToUserEntity(librarianRole!);
            userEntity.Status = AccountStatus.Approved;
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _usersRepository.GetUser(validLibrarian.EmailAddress, "wrong password");
            });
        }

        [Test]
        public async Task GetUser_Fails_InvalidEmail()
        {
            var librarianRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == validLibrarian.RoleName);
            var userEntity = validLibrarian.ToUserEntity(librarianRole!);
            userEntity.Status = AccountStatus.Approved;
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _usersRepository.GetUser("wrong email", validLibrarian.Password);
            });
        }

        [Test]
        public async Task GetUserByEmail()
        {
            var librarianRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == validLibrarian.RoleName);
            var userEntity = validLibrarian.ToUserEntity(librarianRole!);
            userEntity.Status = AccountStatus.Approved;
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            var user = await _usersRepository.GetUser(validLibrarian.EmailAddress);
            Assert.That(user, Is.Not.Null);
            Assert.That(user.FirstName, Is.Not.Null.Or.Empty);
            Assert.That(user.LastName, Is.Not.Null.Or.Empty);
            Assert.That(user.PhoneNumber, Is.Not.Null.Or.Empty);
            Assert.That(user.EmailAddress, Is.Not.Null.Or.Empty);
        }

        [Test]
        public async Task GetUserByEmail_Fails()
        {
            var librarianRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == validLibrarian.RoleName);
            var userEntity = validLibrarian.ToUserEntity(librarianRole!);
            userEntity.Status = AccountStatus.Approved;
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _usersRepository.GetUser("wrong email");
            });
        }

        [Test]
        public async Task ApproveUser()
        {
            await _usersRepository.Register(validCustomer);
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == validCustomer.EmailAddress);

            var approver = Substitute.For<ClaimsPrincipal>();
            approver.IsInRole(Roles.Librarian).Returns(true);

            var approvedUser = await _usersRepository.ApproveUser(userEntity.UserId, approver);
            Assert.That(approvedUser, Is.Not.Null);
            Assert.That(approvedUser.Status, Is.EqualTo(AccountStatus.Approved));
        }

        [Test]
        public async Task ApproveUser_Fails_DoNotHavePermission()
        {
            await _usersRepository.Register(validLibrarian);
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == validLibrarian.EmailAddress);

            var approver = Substitute.For<ClaimsPrincipal>();
            approver.IsInRole(Roles.Librarian).Returns(true);
 
            Assert.ThrowsAsync<InvalidOperationException>(async delegate
            {
                await _usersRepository.ApproveUser(userEntity.UserId, approver);
            });
        }

        [Test]
        public async Task ApproveUser_Fails_WrongId()
        {
            await _usersRepository.Register(validLibrarian);

            var approver = Substitute.For<ClaimsPrincipal>();
            approver.IsInRole(Roles.Librarian).Returns(true);

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _usersRepository.ApproveUser(new Guid(), approver);
            });
        }

        [Test]
        public async Task RejectUser()
        {
            await _usersRepository.Register(validCustomer);
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == validCustomer.EmailAddress);

            var rejecter = Substitute.For<ClaimsPrincipal>();
            rejecter.IsInRole(Roles.Librarian).Returns(true);

            await _usersRepository.RejectUser(userEntity.UserId, rejecter);
            var userExists = await _context.Users.AnyAsync(u => u.UserId == userEntity.UserId);
            Assert.That(userExists, Is.False);
        }

        [Test]
        public async Task RejectUser_Fails_DoNotHavePermission()
        {
            await _usersRepository.Register(validLibrarian);
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == validLibrarian.EmailAddress);

            var rejecter = Substitute.For<ClaimsPrincipal>();
            rejecter.IsInRole(Roles.Librarian).Returns(true);

            Assert.ThrowsAsync<InvalidOperationException>(async delegate
            {
                await _usersRepository.RejectUser(userEntity.UserId, rejecter);
            });
        }

        [Test]
        public async Task RejectUser_Fails_WrongId()
        {
            await _usersRepository.Register(validLibrarian);

            var rejecter = Substitute.For<ClaimsPrincipal>();
            rejecter.IsInRole(Roles.Librarian).Returns(true);

            Assert.ThrowsAsync<ArgumentException>(async delegate
            {
                await _usersRepository.RejectUser(new Guid(), rejecter);
            });
        }
    }
}
