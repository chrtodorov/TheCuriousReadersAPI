using BusinessLayer.Interfaces.Users;
using BusinessLayer.Models;
using DataAccess;
using DataAccess.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
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
                ApartmentNumber = 14,
                BuildingNumber = 1,
                StreetNumber = 15,
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
            _configuration = Substitute.For<IConfiguration>();
            _configuration.GetSection("JwtSecret").Value.Returns("acf484d7-49a9-4e9b-b7eb-9731cabca6c5");
            _context = DbContextHelper.CreateInMemoryDatabase<DataContext>();
            if (_context != null)
            {
                _usersRepository = new UsersRepository(_context, _configuration, _logger);
            }
            await RoleSeeder.SeedRolesAsync(_context);
        }

        [Test]
        public async Task Register()
        {
            var authenticatedUser = await _usersRepository.Register(validLibrarian);
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);

            var librarianExists = await _context.Librarians.AnyAsync();
            Assert.That(librarianExists);


            authenticatedUser = await _usersRepository.Register(validCustomer);
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);

            var customerExists = await _context.Customers.AnyAsync();
            Assert.That(customerExists);


            authenticatedUser = await _usersRepository.Register(validAdmin);
            Assert.That(authenticatedUser.JwtToken, Is.Not.Null.Or.Empty);
            Assert.That(authenticatedUser.RefreshToken, Is.Not.Null.Or.Empty);

            var adminExists = await _context.Administrators.AnyAsync();
            Assert.That(adminExists);
        }

        [Test]
        public void Register_Fails()
        {
            Assert.ThrowsAsync<ArgumentException>(async delegate { await _usersRepository.Register(invalidUser); });
        }

        [Test]
        public async Task Authenticate()
        {
            await _usersRepository.Register(validLibrarian);
            await _usersRepository.Register(validCustomer);
            await _usersRepository.Register(validAdmin);

            var token = await _usersRepository.Authenticate(validLibrarian.EmailAddress, validLibrarian.Password);
            Assert.That(token, Is.Not.Null.Or.Empty);

            token = await _usersRepository.Authenticate(validCustomer.EmailAddress, validCustomer.Password);
            Assert.That(token, Is.Not.Null.Or.Empty);

            token = await _usersRepository.Authenticate(validAdmin.EmailAddress, validAdmin.Password);
            Assert.That(token, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void Authenticate_Fails()
        {
            Assert.ThrowsAsync<ArgumentException>(async delegate { await _usersRepository.Authenticate(invalidUser.EmailAddress, invalidUser.Password); });
        }
    }
}
